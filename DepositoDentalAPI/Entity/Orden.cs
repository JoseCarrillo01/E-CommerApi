using System.ComponentModel.DataAnnotations;

namespace DepositoDentalAPI.Entity
{
    public class Orden
    {
        public int Id { get; set; }

        public DateTime FechaPedido { get; set; }

        public double GranTotal { get; set; }

        public string Estado { get; set; }
        [StringLength(50)]
        public string? Notas { get; set; }

        [StringLength(50)]
        public string? DireccionEntrega { get; set; }

        public DateTime? FechaEntrega { get; set; }

        public int UsuarioId { get; set; }

        public Usuario usuario { get; set; }
    }
}
