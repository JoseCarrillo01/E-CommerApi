using AutoMapper;
using DepositoDentalAPI.DTOs.Categoria;
using DepositoDentalAPI.DTOs.Producto;
using DepositoDentalAPI.DTOs.Usuario;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DepositoDentalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusquedaController : ControllerBase
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IMapper mapper;

        // GET: BusquedaController/Details/5
        public BusquedaController(ApplicationDbContext applicationDbContext,IMapper mapper)
        {
            this.applicationDbContext = applicationDbContext;
            this.mapper = mapper;
        }
       

        // POST: BusquedaController/Create
        [HttpGet("${modelo}/${termino}")]
        public async Task<ActionResult>  buscarTodo(string modelo,string termino)
        {
        

            switch (modelo)
            {
                case "Usuarios":

                    var usuarios = await applicationDbContext.usuarios.
                     Where(x => x.Nombre.Contains(termino) || x.Correo.Contains(termino)).
                     ToListAsync();

                    var usuariosDtos = mapper.Map<List<UsuarioDTO>>(usuarios);

                    return Ok(usuariosDtos);

                    break;

                case "Categorias":
                    var categorias = await applicationDbContext.categorias.
                        Where(x => x.Nombre.Contains(termino) || x.Tipo.Contains(termino)).ToListAsync();

                    var categoriasDtos = mapper.Map<List<CategoriaDTO>>(categorias);
                    return Ok(categoriasDtos);
                    break;

                case "Productos":
                    var productos = await applicationDbContext.productos.
                        Where(x => x.ProductoNombre.Contains(termino)).ToListAsync();

                    var productosDtos = mapper.Map<List<ProductoDTO>>(productos);
                    return Ok(productosDtos);
                    break;
                default:
                    return NotFound("No existe ese modelo");
                    break;
            }

        }

     
    }
}
