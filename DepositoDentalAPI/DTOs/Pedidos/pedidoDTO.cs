namespace DepositoDentalAPI.DTOs.Pedidos
{
    public class pedidoDTO
    {

        public int Id { get; set; }

        
        public int UsuarioId { get; set; }

      
        public int ProductoId { get; set; }

        public string correo { get; set; }

        public string telefono { get; set; }

       
        public int cantidad { get; set; }

       
        public double precio { get; set; }
        public Boolean aceptado { get; set; }
        public Boolean eliminado { get; set; }





    }
}
