using DepositoDentalAPI.DTOs.Auth;
using DepositoDentalAPI.DTOs.Usuario;
using DepositoDentalAPI.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DepositoDentalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public AuthController(IUsuarioService usuarioService)
        {
            this._usuarioService = usuarioService;
        }

        [HttpPost("registro")]
        public async Task<ActionResult<UsuarioDTO>> RegistrarUsuario([FromBody] UsuarioCreacionDTO usuarioCreacionDTO)
        {
           if(usuarioCreacionDTO is null)
            {
                return BadRequest("Rellenar todos los campos");
            }
            var entidad = await _usuarioService.registrar(usuarioCreacionDTO);

            return Ok(entidad);
        }

        // Login
        [HttpPost("login")]
        public  async Task<ActionResult<string>> Login([FromBody] LoginDTO model)
        {
            string respuesta = System.String.Empty;

            respuesta = await _usuarioService.auth(model);

            if(respuesta == null)
            {
                return BadRequest("Usuario o contraseña incorrecta");
            }
        
            return Ok(respuesta);
        }


    }
}
