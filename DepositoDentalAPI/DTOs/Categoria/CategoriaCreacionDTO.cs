using System.ComponentModel.DataAnnotations;

namespace DepositoDentalAPI.DTOs.Categoria
{
    public class CategoriaCreacionDTO
    {

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        public IFormFile? Imagen { get; set; }

        [Required]
        public string Descripcion { get; set; }

        public string Tipo { get; set; }
    }
}
