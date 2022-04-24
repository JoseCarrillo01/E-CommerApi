using AutoMapper;
using DepositoDentalAPI.Entity;
using DepositoDentalAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DepositoDentalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext dbContext;

        public CustomBaseController(IMapper mapper, ApplicationDbContext dbContext)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        //entidadDTO    //ENTIDAD  entidadDTO
        protected async Task<List<TDTO>> GetBase<TEntidad, TDTO>() where TEntidad : class
        {
            var entidades = await dbContext.Set<TEntidad>().AsNoTracking().ToListAsync();//Se trabajara con cualquier entidad
            var dtos = mapper.Map<List<TDTO>>(entidades); //mapeo de cualquier entidad
            return dtos;
        }

        // GET api/<CustomBaseController>/5
        //Can be use it in all controllers
        protected async Task<ActionResult<TDTO>> GetByIdBase<TEntidad, TDTO>(int id) where TEntidad : class, Iid
        {
            var entidad = await dbContext.Set<TEntidad>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (entidad == null)
            {
                return NotFound();
            }

            return mapper.Map<TDTO>(entidad);
        }

        protected async Task<ActionResult> PostBase<TCreacion, TEntidad, TLectura>
               (TCreacion creacionDTO, string nombreRuta,string rutaImagen) where TEntidad : class, Iid,Iimagen
        {
            var entidad = mapper.Map<TEntidad>(creacionDTO); //Mappeamos creacionDTO A generico Entidad 

            if (rutaImagen != null && rutaImagen != "")
            {
                entidad.Imagen = rutaImagen;
            }

            dbContext.Add(entidad);

            await dbContext.SaveChangesAsync();

            var dtoLectura = mapper.Map<TLectura>(entidad); //Mapeamamos generico entidad a generico entidad Lectura

            return new CreatedAtRouteResult(nombreRuta, new { id = entidad.Id }, dtoLectura);
        }


        protected async Task<ActionResult> PutBase<TCreacion, TEntidad>
             (int id, TCreacion creacionDTO, string rutaImagen) where TEntidad : class, Iid,Iimagen
        {
            var existe = await dbContext.Set<TEntidad>().AsNoTracking().AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound("PutBase no existe entidad con ese Id");
            }

            var entidad = mapper.Map<TEntidad>(creacionDTO);

            if (rutaImagen != null && rutaImagen != "")
            {
                entidad.Imagen = rutaImagen;
            }

            entidad.Id = id;

            dbContext.Entry(entidad).State = EntityState.Modified;

            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        protected async Task<ActionResult> DeleteBase<TEntidad>(int id) where TEntidad : class, Iid, new()
        {
            var existe = await dbContext.Set<TEntidad>().AnyAsync(context => context.Id == id);

            if (!existe)
            {
                return NotFound();
            }


            dbContext.Remove(new TEntidad() { Id = id });

            await dbContext.SaveChangesAsync();

            return NoContent();

        }
    }
}
