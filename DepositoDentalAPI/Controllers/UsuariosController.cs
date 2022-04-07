using AutoMapper;
using DepositoDentalAPI.DTOs.Categoria;
using DepositoDentalAPI.DTOs.Usuario;
using DepositoDentalAPI.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DepositoDentalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : CustomBaseController
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext dbContext;

        public UsuariosController(IMapper mapper, ApplicationDbContext dbContext) : base(mapper, dbContext)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<UsuarioDTO>>> GetAll()
        { 
            return await GetBase<Usuario,UsuarioDTO>();
        }

        // GET api/<UsuariosController>/5
        [HttpGet("{id}", Name = "getUsuario")]
        public async Task<ActionResult<UsuarioDTO>> Get(int id)
        {

            return await GetByIdBase<Usuario, UsuarioDTO>(id);
        }

        // PUT api/<UsuariosController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] UsuarioCreacionDTO usuarioCreacionDTO)
        {
            var existe = await dbContext.usuarios.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            var autor = mapper.Map<Usuario>(usuarioCreacionDTO);
            autor.Id = id;

            dbContext.Update(autor);
            await dbContext.SaveChangesAsync();
            return NoContent();

        }

        // DELETE api/<UsuariosController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await DeleteBase<Usuario>(id);
        }
    }
}
