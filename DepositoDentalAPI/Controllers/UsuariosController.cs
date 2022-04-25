using AutoMapper;
using DepositoDentalAPI.DTOs.Categoria;
using DepositoDentalAPI.DTOs.DetalleOrden;
using DepositoDentalAPI.DTOs.Orden;
using DepositoDentalAPI.DTOs.Usuario;
using DepositoDentalAPI.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DepositoDentalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : CustomBaseController
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext dbContext;

        public UsuariosController(IMapper mapper, ApplicationDbContext dbContext) 
            : base(mapper, dbContext)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<List<UsuarioDTO>>> GetAll()
        { 
            return await GetBase<Usuario,UsuarioDTO>();
        }

        [HttpGet("rolUsuario")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Dictionary<string, string> GetRolUsuario()
        {
            var TokenInfo = new Dictionary<string, string>();

            var token = Request.Cookies["X-Access-Token"];
            Console.WriteLine("Rol de usuario"+token);

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var claims = jwtSecurityToken.Claims.ToList();
            foreach (var claim in claims)
            {
                TokenInfo.Add(claim.Type, claim.Value);
            }

            return TokenInfo;

        }


        // GET api/<UsuariosController>/5
        [HttpGet("{id}", Name = "getUsuario")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<UsuarioDetalleDTO>> Get(int id)
        {
            return await GetByIdBase<Usuario, UsuarioDetalleDTO>(id);
        }

        // PUT api/<UsuariosController>/5
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Usuario")]
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            


            return await DeleteBase<Usuario>(id);
        }

       

    }
}
