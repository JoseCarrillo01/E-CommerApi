using DepositoDentalAPI.DTOs.Producto;

namespace DepositoDentalAPI.DTOs.DetalleOrden
{
    public class DetalleOrdenDTO
    {
        public int Id { get; set; }
        public double PrecioUnitario { get; set; }

        public int Cantidad { get; set; }
        public int ProductoId { get; set; }
        public int OrdenId { get; set; }

        //Producto
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
        public double Precio {get; set; }
        
        public string ProductoNombre {get;set; }
    }
}
