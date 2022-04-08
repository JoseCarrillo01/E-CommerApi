using AutoMapper;
using DepositoDentalAPI.DTOs.Categoria;
using DepositoDentalAPI.DTOs.DetalleOrden;
using DepositoDentalAPI.DTOs.Orden;
using DepositoDentalAPI.DTOs.Producto;
using DepositoDentalAPI.DTOs.Usuario;
using DepositoDentalAPI.Entity;

namespace DepositoDentalAPI.Helpers
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            //Categoria
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            CreateMap<CategoriaCreacionDTO, Categoria>().ReverseMap();

            //Producto
            CreateMap<Producto,ProductoDTO>().ReverseMap();

            CreateMap<Producto, ProductoDetalleDTO>()
                .ForMember(x => x.categorias, options => options.MapFrom(MapCategoriaProducto));
            //source //destiny
            CreateMap<ProductoCreacionDTO, Producto>()
                .ForMember(x => x.categoriaProductos, options => options.MapFrom(MapCategoriaProductos));

            //Orden
            CreateMap<OrdenCreacionDTO, Orden>().ReverseMap();
            CreateMap<Orden, OrdenDTO>().ReverseMap();

            //Usuarios
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
            CreateMap<UsuarioCreacionDTO, Usuario>().ReverseMap();

            //DetalleOrden
            CreateMap<DetalleOrden, DetalleOrdenDTO>();


        }



        //Metodo para mapear la List<CategoriaDTO> dentro de productoDetalleDTO
        private List<CategoriaDTO> MapCategoriaProducto(Producto producto, ProductoDTO productoDTO)
        {
            var resultado = new List<CategoriaDTO>();

            if (producto.categoriaProductos == null)
            {
                return resultado;
            }

            foreach (var categoriaDelProducto in producto.categoriaProductos)
            {
                resultado.Add(new CategoriaDTO() 
                { 
                    Id = categoriaDelProducto.categoriaId, 
                    Nombre = categoriaDelProducto.categoria.Nombre,
                    Descripcion = categoriaDelProducto.categoria.Descripcion,
                    Imagen = categoriaDelProducto.categoria.Imagen,
                    Tipo = categoriaDelProducto.categoria.Tipo
                });
            }

            return resultado;
        }

        //Metodo para mapear la List<CategoriaProducto> al momento de crear un producto
        private List<CategoriaProducto> MapCategoriaProductos
            (ProductoCreacionDTO productoCreacionDTO, Producto producto)
        {
            var resultado = new List<CategoriaProducto>();

            if (productoCreacionDTO.categoriaProductosIds == null)
            {
                return resultado;
            }

            foreach (var idCategoria in productoCreacionDTO.categoriaProductosIds)
            {
                resultado.Add(new CategoriaProducto()
                {
                    categoriaId = idCategoria
                   
                }) ;
            }

            return resultado;
        }
    }
}
