namespace DepositoDentalAPI.Entity
{
    public class Pedido : Iid
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario usuario { get; set; }
        public int ProductoId { get; set; }
        public Producto producto { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public int cantidad { get; set; }
        public double precio { get; set; }
        public Boolean aceptado { get; set; }
        public Boolean eliminado { get; set; }

    }
}
