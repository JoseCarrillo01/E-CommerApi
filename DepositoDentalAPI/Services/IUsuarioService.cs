using DepositoDentalAPI.DTOs.Auth;
using DepositoDentalAPI.DTOs.Usuario;
using DepositoDentalAPI.Entity;

namespace DepositoDentalAPI.Services
{
    public interface IUsuarioService
    {
        public Task<string> auth(LoginDTO modelo);

        public Task<UsuarioDTO> registrar(UsuarioCreacionDTO modelo);
    }
}
