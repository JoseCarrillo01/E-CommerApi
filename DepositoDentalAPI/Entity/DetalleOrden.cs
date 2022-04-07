namespace DepositoDentalAPI.Entity
{
    public class DetalleOrden
    {

        public int Id { get; set; }
        public double PrecioUnitario { get; set; }

        public int Cantidad { get; set; }
        public int ProductoId { get; set; }
        public Producto producto { get; set; }

        public int OrdenId { get; set; }

        public Orden orden { get; set; }
    }
}
