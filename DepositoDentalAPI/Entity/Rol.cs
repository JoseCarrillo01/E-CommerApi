using System.ComponentModel.DataAnnotations;

namespace DepositoDentalAPI.Entity
{
    public class Rol
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(30)]
        public string Nombre { get; set; }
    }
}
