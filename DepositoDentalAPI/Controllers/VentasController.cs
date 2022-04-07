using AutoMapper;
using Braintree;
using DepositoDentalAPI.DTOs.DetalleOrden;
using DepositoDentalAPI.DTOs.Orden;
using DepositoDentalAPI.Entity;
using DepositoDentalAPI.Services;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPost]
        public async Task<ActionResult> Post(OrdenCreacionDTO ordenCreacionDTO)
        {
            Console.WriteLine(ordenCreacionDTO.ToString());

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

            Result<Transaction> result = gateway.Transaction.Sale(request);

            if (!result.IsSuccess())
            {
                return BadRequest(result);

            }

            return Ok(new {result,entidad});

        }
    }

    
}
