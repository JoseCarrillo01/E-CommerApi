using AutoMapper;
using DepositoDentalAPI.DTOs.Categoria;
using DepositoDentalAPI.DTOs.DetalleOrden;
using DepositoDentalAPI.DTOs.Orden;
using DepositoDentalAPI.DTOs.Usuario;
using DepositoDentalAPI.Entity;
using DepositoDentalAPI.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UsuarioDetalleDTO>> Get(int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var IdClaim = "";
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                // or
                IdClaim = identity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;

            }
            if(id.ToString() != IdClaim)
            {
                return BadRequest("No tienes permiso para realizar esta accion");
            }
            return await GetByIdBase<Usuario, UsuarioDetalleDTO>(id);
        }

        // TODOS 
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(int id, [FromBody] UsuarioCreacionDTO usuarioCreacionDTO)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var IdClaim = "";
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                // or
                IdClaim = identity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;

            }
            if (id.ToString() != IdClaim)
            {
                return BadRequest("No tienes permiso para realizar esta accion");
            }

            var password = usuarioCreacionDTO.Password;
            usuarioCreacionDTO.Password = Encriptar256.GetSHA256(password);
            var existe = await dbContext.usuarios.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            var autor = mapper.Map<Usuario>(usuarioCreacionDTO);
            autor.Id = id;
            autor.RolId = 2;
            dbContext.Update(autor);
            await dbContext.SaveChangesAsync();
            return NoContent();

        }

        // DELETE api/<UsuariosController>/5
        //TODOS
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(int id)
        {


            return await DeleteBase<Usuario>(id);
        }

       

    }
}
