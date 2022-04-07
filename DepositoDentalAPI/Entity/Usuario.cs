using System.ComponentModel.DataAnnotations;

namespace DepositoDentalAPI.Entity
{
    public class Usuario:Iid
    {
        public int Id { get; set; }

        [StringLength(100)]
        [Required]
        public string Nombre { get; set; }

        [StringLength(10)]
        [MinLength(10)]
        public string celular { get; set; }

        [StringLength(50)]
        public string Direccion { get; set; }
        public string CodigoPostal { get; set; }

        [EmailAddress(ErrorMessage ="Error Email Invalido")]
        public string Correo { get; set; }

        [StringLength(13)]
        public string RFC { get; set; }

        [MinLength(6)]
        public string Password { get; set; }

        public int RolId { get; set; }

        public Rol rol { get; set; }

    }
}
