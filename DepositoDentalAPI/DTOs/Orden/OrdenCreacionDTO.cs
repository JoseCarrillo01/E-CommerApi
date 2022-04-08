using DepositoDentalAPI.DTOs.DetalleOrden;
using DepositoDentalAPI.Entity;
using System.ComponentModel.DataAnnotations;

namespace DepositoDentalAPI.DTOs.Orden
{
    public class OrdenCreacionDTO
    {
        public DateTime FechaPedido { get; set; }

        public double GranTotal { get; set; }

        public string? Estado { get; set; }

        [StringLength(50)]
        public string? Notas { get; set; }

        [StringLength(50)]
        public string? DireccionEntrega { get; set; }

        public DateTime? FechaEntrega { get; set; }

        public string? nonce { get; set; }

        public int UsuarioId { get; set; }

        public List<DetalleOrdenCreacionDTO>? detalles { get; set; }

    }
}
