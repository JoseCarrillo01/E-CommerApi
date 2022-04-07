using AutoMapper;
using DepositoDentalAPI.DTOs.Auth;
using DepositoDentalAPI.DTOs.Usuario;
using DepositoDentalAPI.Entity;
using DepositoDentalAPI.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DepositoDentalAPI.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppSettings _appSettings;
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public UsuarioService(IOptions<AppSettings> appSettings,ApplicationDbContext dbContext,IMapper mapper)
        {
            _appSettings = appSettings.Value;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        public async Task<string> auth(LoginDTO modelo)
        {

            string passwordLogin = Encriptar256.GetSHA256(modelo.Password);

           var usuario =  await dbContext.usuarios.Include(x=>x.rol).Where(x => x.Correo == modelo.Email && x.Password == passwordLogin).FirstOrDefaultAsync();
           
            if (usuario == null) return null;

            return ConstruirToken(usuario); 

        }

        public async Task<UsuarioDTO> registrar(UsuarioCreacionDTO usuarioCreacionDTO)
        {
            var password = usuarioCreacionDTO.Password;
            usuarioCreacionDTO.Password = Encriptar256.GetSHA256(password);

            var entidad = mapper.Map<Usuario>(usuarioCreacionDTO);
            try
            {
                entidad.RolId = 2;
                
                dbContext.Add(entidad);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            var entidadDTO = mapper.Map<UsuarioDTO>(entidad);

            return entidadDTO;

        }

        private string ConstruirToken(Usuario usuario)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Correo),
                new Claim(ClaimTypes.Role,usuario.rol.Nombre) //No se recomienda pero en este caso funciona sin errores

            };

            var llave = Encoding.ASCII.GetBytes(_appSettings.SecretJWT);

     

            var key = new SymmetricSecurityKey(llave);

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddMinutes(20);

            JwtSecurityToken token = new JwtSecurityToken(
              issuer: null,
              audience: null,
              claims: claims,
              expires: expiracion,
              signingCredentials: creds
          );


            return new JwtSecurityTokenHandler().WriteToken(token);
              
           
        }

    }
}
