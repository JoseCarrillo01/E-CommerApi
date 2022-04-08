using AutoMapper;
using Braintree;
using DepositoDentalAPI.DTOs.DetalleOrden;
using DepositoDentalAPI.DTOs.Orden;
using DepositoDentalAPI.Entity;
using DepositoDentalAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DepositoDentalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly IBraintreeService _braintreeService;
        private readonly IMapper mapper;
        private readonly ApplicationDbContext dbContext;

        public VentasController(IBraintreeService braintreeService,IMapper mapper,ApplicationDbContext dbContext)
        {
            _braintreeService = braintreeService;
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        [HttpGet]
        public TokenPago GetClientToken()
        {
            var gateway = _braintreeService.GetGateway();
            var token = gateway.ClientToken.Generate();  //Genarate a token

            var TokenPago = new TokenPago()
            {
                token = token,
            };

            return TokenPago;
        }

        // POST api/<PagosController>
        [HttpPost] //Final
        public async Task<ActionResult> PostPreVenta(OrdenCreacionDTO ordenCreacionDTO)
        {

            if (ordenCreacionDTO == null)
            {
                return BadRequest();
            }

            var entidad = mapper.Map<Orden>(ordenCreacionDTO); //Mappeamos creacionDTO A generico Entidad 

            dbContext.ordenes.Add(entidad);

            await dbContext.SaveChangesAsync();

            foreach (var detallesLista in ordenCreacionDTO.detalles)
            {
                var detalleOrden = new DetalleOrden() { };
                detalleOrden.OrdenId = entidad.Id;
                detalleOrden.ProductoId = detallesLista.ProductoId;
                detalleOrden.Cantidad = detallesLista.Cantidad;
                detalleOrden.PrecioUnitario = detallesLista.PrecioUnitario;
                dbContext.detallesOrden.Add(detalleOrden);
                await dbContext.SaveChangesAsync();

            }
            return Ok();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVenta(int id)
        {
            var existe = await dbContext.ordenes.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            dbContext.Remove(new Orden() { Id = id });
            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("generarVenta/{id}")] //Final
        public async Task<ActionResult> PostVenta(int id,OrdenCreacionDTO ordenCreacionDTO)
        {
            Console.WriteLine(id);

            Console.WriteLine(ordenCreacionDTO.ToString());

            var existe = await dbContext.ordenes.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            if (ordenCreacionDTO == null)
            {
                return BadRequest();
            }


            var gateway = _braintreeService.GetGateway();

            var request = new TransactionRequest
            {
                Amount = Convert.ToDecimal(ordenCreacionDTO.GranTotal),
                PaymentMethodNonce = ordenCreacionDTO.nonce,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            var orden = mapper.Map<Orden>(ordenCreacionDTO);
            orden.Id = id;

            dbContext.Update(orden);

            await dbContext.SaveChangesAsync();

            Result<Transaction> result = gateway.Transaction.Sale(request);

            if (!result.IsSuccess())
            {
                return BadRequest(result);

            }

            return Ok(result);

        }
    }

    
}
