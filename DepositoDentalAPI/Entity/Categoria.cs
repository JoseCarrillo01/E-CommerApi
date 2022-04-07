using System.ComponentModel.DataAnnotations;

namespace DepositoDentalAPI.Entity
{
    public class Categoria:Iid,Iimagen
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        public string? Imagen { get; set; }

        [Required]
        public string Descripcion { get; set; }

        public string Tipo { get; set; }

        public List<CategoriaProducto> categoriaProductos { get; set; }


    }
}
