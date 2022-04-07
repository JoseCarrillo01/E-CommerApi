using DepositoDentalAPI.DTOs.Categoria;

namespace DepositoDentalAPI.DTOs.Producto
{
    public class ProductoDetalleDTO:ProductoDTO
    {
        public List<CategoriaDTO> categorias { get; set; }
    }
}
