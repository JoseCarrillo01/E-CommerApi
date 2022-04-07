using DepositoDentalAPI.Entity;
using System.ComponentModel.DataAnnotations;

namespace DepositoDentalAPI.DTOs.Usuario
{
    public class UsuarioCreacionDTO
    {

        [StringLength(100)]
        [Required]
        public string Nombre { get; set; }

        [StringLength(10)]
        [MinLength(10)]
        public string celular { get; set; }

        [StringLength(50)]
        [Required]
        public string Direccion { get; set; }

        [MinLength(5)]
        [Required]
        public string CodigoPostal { get; set; }

        [EmailAddress(ErrorMessage = "Error Email Invalido")]
        [Required]
        public string Correo { get; set; }

        [StringLength(13)]
        [Required]
        public string RFC { get; set; }

        [MinLength(6)]
        [Required]
        public string Password { get; set; }
    }
}
