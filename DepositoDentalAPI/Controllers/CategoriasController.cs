using AutoMapper;
using DepositoDentalAPI.DTOs.Categoria;
using DepositoDentalAPI.Entity;
using DepositoDentalAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DepositoDentalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : CustomBaseController
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext dbContext;
        private readonly ICloudinaryService cloudinary;

        public CategoriasController(IMapper mapper, ApplicationDbContext dbContext,ICloudinaryService cloudinary)
            : base(mapper, dbContext)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
            this.cloudinary = cloudinary;
        }

        // GET: api/<CategoriaController>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Admin")]
        public async Task<ActionResult<List<CategoriaDTO>>> Get()
        {
            return await GetBase<Categoria, CategoriaDTO>();
        }

        // GET api/<CategoriaController>/5
        [HttpGet("{id}", Name = "getCategoria")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {

            return await GetByIdBase<Categoria,CategoriaDTO>(id);
        }

        // POST api/<CategoriaController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CategoriaCreacionDTO categoriaCreacionDTO)
        {
            var rutaImagen = "";

            if (categoriaCreacionDTO is null)
            {
                return BadRequest();
            }                           //Destiny // source

            if (categoriaCreacionDTO.Imagen is not null)
            {

                using (var memoryStream = new MemoryStream())
                {
                    await categoriaCreacionDTO.Imagen.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    Stream stream = new MemoryStream(content);
                    var resultado = cloudinary.subirImagen(stream);
                    rutaImagen = resultado.ToString();
                }

            }             //Destiny // source

            return await PostBase<CategoriaCreacionDTO, Categoria, CategoriaDTO>(categoriaCreacionDTO, "getCategoria",rutaImagen);
        }

        // PUT api/<CategoriaController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CategoriaCreacionDTO categoriaCreacionDTO)
        {
            var rutaImagen = "";
            var entidad = await dbContext.categorias.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (entidad is null)
            {
                return NotFound("No existe ninguna categoria con ese Id");
            }

            //si la imagen guardada en la base de datos no esta vacia
            if (entidad.Imagen is not null)
            {
                var nombreArr = entidad.Imagen.Split("/");
                var nombre = nombreArr[nombreArr.Length - 1];
                var idCloudArr = nombre.Split(".");
                var idCloud = idCloudArr[0];
                cloudinary.borrarImagen(idCloud); //borramos la antigua foto
            }


            if (categoriaCreacionDTO is null)
            {
                return BadRequest(); //faltan datos
            }

            //si nuestra imagen auxiliar no esta vacia
            if (categoriaCreacionDTO.Imagen is not null)
            {

                using (var memoryStream = new MemoryStream())
                {
                    await categoriaCreacionDTO.Imagen.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    Stream stream = new MemoryStream(content);
                    var resultado = cloudinary.subirImagen(stream);
                    rutaImagen = resultado.ToString();
                }
                //Insertamos foto
            }


            return await PutBase<CategoriaCreacionDTO, Categoria>(id, categoriaCreacionDTO, rutaImagen);

        }

        // DELETE api/<CategoriaController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {

            return await DeleteBase<Categoria>(id);
        }
    }
}
