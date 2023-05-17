using AutoMapper;
using DepositoDentalAPI.DTOs.Pedidos;
using DepositoDentalAPI.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace DepositoDentalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tablapedidos : ControllerBase
    {

         
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IMapper mapper;

        public tablapedidos(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            this.applicationDbContext = applicationDbContext;
            this.mapper = mapper;
        }

        // GET: api/<tablapedidos>
        [HttpGet]
        public async Task<List<Pedido>> Get()
        {
            var entidades = await applicationDbContext.Set<Pedido>().ToListAsync();//Se trabajara con cualquier entidad

            return entidades;
        }

        // GET api/<tablapedidos>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<pedidoDTO>> Get(int id)
        {
            var entidad = await applicationDbContext.Set<Pedido>().FirstOrDefaultAsync(x => x.Id == id);
            if (entidad == null)
            {
                return NotFound();
            }

            return mapper.Map<pedidoDTO>(entidad);
        }

        // POST api/<tablapedidos>
        [HttpPost]
        public async Task<pedidoDTO> Post([FromBody] CrearPedidoDTO value)
        {
            var pedido = mapper.Map<Pedido>(value);


            applicationDbContext.Add(pedido);
            await applicationDbContext.SaveChangesAsync();
            var PedidoDTO = mapper.Map<pedidoDTO>(pedido);

            return PedidoDTO;
        }

        // PUT api/<tablapedidos>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<tablapedidos>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }




        [HttpGet("EliminarPedido/{id}")]
        public async Task<ActionResult<pedidoDTO>> EliminarPedido(int id)
        {
            var entidad = await applicationDbContext.Set<Pedido>().FirstOrDefaultAsync(x => x.Id == id);
            if (entidad == null)
            {
                return NotFound();
            }
            entidad.aceptado = false;
            entidad.eliminado = true;
            applicationDbContext.Entry(entidad).State = EntityState.Modified;
            await applicationDbContext.SaveChangesAsync();

            return mapper.Map<pedidoDTO>(entidad);
        }



        [HttpGet("AcceptarPedido/{id}")]
        public async Task<ActionResult<pedidoDTO>> AcceptarPedido(int id)
        {
            var entidad = await applicationDbContext.Set<Pedido>().FirstOrDefaultAsync(x => x.Id == id);
            if (entidad == null)
            {
                return NotFound();
            }
            entidad.aceptado = true;
            entidad.eliminado = false;
            applicationDbContext.Entry(entidad).State = EntityState.Modified;
            await applicationDbContext.SaveChangesAsync();


            return mapper.Map<pedidoDTO>(entidad);
        }















    }
}
