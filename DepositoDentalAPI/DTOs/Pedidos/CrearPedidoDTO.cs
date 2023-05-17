using System.ComponentModel.DataAnnotations;

namespace DepositoDentalAPI.DTOs.Pedidos
{
    public class CrearPedidoDTO
    {

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public int ProductoId { get; set; }

        [StringLength(100)]
        [Required]
        public string correo { get; set; }

        [StringLength(100)]
        [Required]
        public string telefono { get; set; }

        [Required]
        public int cantidad { get; set; }

        [Required]
        public double precio { get; set; }
        public Boolean aceptado { get; set; }

        public Boolean eliminado { get; set; }




    }
}
