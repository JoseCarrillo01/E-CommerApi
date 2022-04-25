using AutoMapper;
using DepositoDentalAPI.DTOs.Producto;
using DepositoDentalAPI.Entity;
using DepositoDentalAPI.Helpers;
using DepositoDentalAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DepositoDentalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : CustomBaseController
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext dbContext;
        private readonly ICloudinaryService cloudinary;

        public ProductosController(IMapper mapper, ApplicationDbContext dbContext,ICloudinaryService cloudinary) : base(mapper, dbContext)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
            this.cloudinary = cloudinary;
        }
        // GET: api/<CategoriaController>
        [HttpGet]
        public async Task<ActionResult<List<ProductoDTO>>> Get()
        {
            var entidades = await dbContext.productos.Include(x => x.categoriaProductos)
                .ThenInclude(x=>x.categoria)
                .ToListAsync();
                
               
            var dtos = mapper.Map<List<ProductoDTO>>(entidades); //mapeo de cualquier entidad
            return dtos;
        }

      

        // GET api/<CategoriaController>/5
        [HttpGet("{id}", Name = "getProducto")]
        public async Task<ActionResult<ProductoDTO>> Get(int id)
        {
            //entidad
            var existeProd = await dbContext.productos
              .Include(x => x.categoriaProductos)
                .ThenInclude(x => x.categoria)
                .FirstOrDefaultAsync(x => x.Id == id);

            if(existeProd == null)
            {
                return NotFound();
            }

            return  mapper.Map<ProductoDetalleDTO>(existeProd);
        }

        // POST api/<CategoriaController>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Post([FromForm] ProductoCreacionDTO productoCreacionDTO)
        {
            Console.WriteLine(productoCreacionDTO);
            var rutaImagen = "";

            if (productoCreacionDTO is null)
            {
                return BadRequest();
            }                           //Destiny // source

            if (productoCreacionDTO.Imagen is not null)
            {

                using (var memoryStream = new MemoryStream())
                {
                    await productoCreacionDTO.Imagen.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    Stream stream = new MemoryStream(content);
                    var resultado = cloudinary.subirImagen(stream);
                    rutaImagen= resultado.ToString();
                }

            }

            return await PostBase<ProductoCreacionDTO, Producto, ProductoDTO>
                (productoCreacionDTO, "getCategoria",rutaImagen);

        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Put(int id, [FromForm] ProductoCreacionDTO productoCreacionDTO)
        {
            var rutaImagen = "";
            var entidad = await dbContext.productos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            
            if(entidad is null)
            {
                return NotFound("No existe ningun producto con ese Id");
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
            

            if (productoCreacionDTO is null)
            {
                return BadRequest(); //faltan datos
            }                           

            //si nuestra imagen auxiliar no esta vacia
            if (productoCreacionDTO.Imagen is not null)
            {

                using (var memoryStream = new MemoryStream())
                {
                    await productoCreacionDTO.Imagen.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    Stream stream = new MemoryStream(content);
                    var resultado = cloudinary.subirImagen(stream);
                    rutaImagen = resultado.ToString();
                }
                //Insertamos foto
            }            
            return await PutBase<ProductoCreacionDTO, Producto>(id, productoCreacionDTO,rutaImagen);
        }

        // DELETE api/<CategoriaController>/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            //Comentario Para arreglar heroku
            var entidad = await dbContext.productos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (entidad == null)
            {
                return NotFound();
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

            dbContext.Remove(new Producto() { Id = id });
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
        
    }
}
