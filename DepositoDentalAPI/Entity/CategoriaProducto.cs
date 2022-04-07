namespace DepositoDentalAPI.Entity
{
    public class CategoriaProducto
    {

        public int categoriaId { get; set; }
        public int productoId { get; set; }
        //propiedades de navegacion
        public Producto producto { get; set; }
        public Categoria categoria { get; set; }
    }
}
