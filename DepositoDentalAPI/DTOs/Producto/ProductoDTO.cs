namespace DepositoDentalAPI.DTOs.Producto
{
    public class ProductoDTO
    {
        public int Id { get; set; }

        public string ProductoNombre { get; set; }

        public string Descripcion { get; set; }

        public int? CantidadEnStock { get; set; }

        public double Precio { get; set; }

        public string? Imagen { get; set; }

        public List<int> categoriaId { get; set; }
    }
}
