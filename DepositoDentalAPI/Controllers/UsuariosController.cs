using AutoMapper;
using DepositoDentalAPI.DTOs.Categoria;
using DepositoDentalAPI.DTOs.DetalleOrden;
using DepositoDentalAPI.DTOs.Orden;
using DepositoDentalAPI.DTOs.Usuario;
using DepositoDentalAPI.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DepositoDentalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : CustomBaseController
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext dbContext;

        public UsuariosController(IMapper mapper, ApplicationDbContext dbContext) 
            : base(mapper, dbContext)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<List<UsuarioDTO>>> GetAll()
        { 
            return await GetBase<Usuario,UsuarioDTO>();
        }

        // GET api/<UsuariosController>/5
        [HttpGet("{id}", Name = "getUsuario")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<UsuarioDTO>> Get(int id)
        {

            return await GetByIdBase<Usuario, UsuarioDTO>(id);
        }

        // PUT api/<UsuariosController>/5
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Usuario")]
        public async Task<ActionResult> Put(int id, [FromBody] UsuarioCreacionDTO usuarioCreacionDTO)
        {
            var existe = await dbContext.usuarios.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            var autor = mapper.Map<Usuario>(usuarioCreacionDTO);
            autor.Id = id;

            dbContext.Update(autor);
            await dbContext.SaveChangesAsync();
            return NoContent();

        }

        // DELETE api/<UsuariosController>/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            return await DeleteBase<Usuario>(id);
        }

        [HttpGet("Facturas/{id}")]
       //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Usuario")]

        public async Task<ActionResult<List<OrdenDTO>>> GetAllFacturas(int id)
        {
            var existeId = await dbContext.usuarios.AnyAsync(x => x.Id == id);

            if (!existeId)
            {
                return BadRequest("No existe un usuario con ese Id");
            }
            //To do
            var ordenes = await dbContext.ordenes.Where(x => x.UsuarioId == id).ToListAsync();

            var ordenesDto = mapper.Map<List<OrdenDTO>>(ordenes);

            return Ok(ordenesDto);
        }

        [HttpGet("DetalleFactura/{idFactura}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Usuario")]

        public async Task<ActionResult<List<DetalleOrdenDTO>>> GetDetalleFactura(int idFactura)
        {
            var existeId = await dbContext.ordenes.AnyAsync(x => x.Id == idFactura);

            if (!existeId)
            {
                return BadRequest("No existe una factura con ese Id");
            }

            //To do
            var detalleOrdenes = await dbContext.detallesOrden
                .Select(detalles => new DetalleOrdenDTO
                {
                    Id = detalles.Id,
                    Cantidad = detalles.Cantidad,
                    ProductoId = detalles.ProductoId,
                    PrecioUnitario = detalles.PrecioUnitario,
                    OrdenId = detalles.OrdenId,

                    Descripcion = detalles.producto.Descripcion,
                    Imagen = detalles.producto.Imagen,
                    Precio = detalles.producto.Precio,
                    ProductoNombre = detalles.producto.ProductoNombre 
                    
                }).Where(x => x.OrdenId == idFactura).ToListAsync();


            return Ok(detalleOrdenes);
        }


    }
}
