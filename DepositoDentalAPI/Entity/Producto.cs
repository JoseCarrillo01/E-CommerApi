using System.ComponentModel.DataAnnotations;

namespace DepositoDentalAPI.Entity
{
    public class Producto:Iid,Iimagen
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductoNombre { get; set; }

        public string Descripcion { get; set; }

        public int? CantidadEnStock { get; set; }

        public double Precio { get; set; }

        public string? Imagen { get; set; }

        public List<CategoriaProducto> categoriaProductos { get; set; }

    }
}
