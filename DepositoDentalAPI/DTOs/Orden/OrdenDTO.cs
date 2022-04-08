namespace DepositoDentalAPI.DTOs.Orden
{
    public class OrdenDTO
    {
        public int Id { get; set; }

        public DateTime FechaPedido { get; set; }

        public double GranTotal { get; set; }

        public string Estado { get; set; }
        public string? Notas { get; set; }

        public string? DireccionEntrega { get; set; }

        public DateTime? FechaEntrega { get; set; }

        public int UsuarioId { get; set; }

    }
}
