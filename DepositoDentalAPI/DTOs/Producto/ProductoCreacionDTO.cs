using DepositoDentalAPI.Entity;
using DepositoDentalAPI.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DepositoDentalAPI.DTOs.Producto
{
    public class ProductoCreacionDTO
    {


        [Required]
        [StringLength(100)]
        public string ProductoNombre { get; set; }

        public string Descripcion { get; set; }

        public int? CantidadEnStock { get; set; }

        public double Precio { get; set; }

        public IFormFile? Imagen { get; set; }

        [ModelBinder(BinderType =typeof(ModelBinderType<List<int>>))]
        public List<int> categoriaProductosIds { get; set; }
    }
}
