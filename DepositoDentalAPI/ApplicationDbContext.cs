using DepositoDentalAPI.Entity;
using Microsoft.EntityFrameworkCore;

namespace DepositoDentalAPI
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {
 
        }


     protected override void OnModelCreating(ModelBuilder modelBuilder) { 
            {
                modelBuilder.Entity<CategoriaProducto>()
                    .HasKey(x => new { x.categoriaId, x.productoId });

                base.OnModelCreating(modelBuilder);
            }
        }

        public DbSet<Categoria> categorias { get; set; }
        public DbSet<Rol> roles { get; set; }
        public DbSet<Usuario> usuarios { get; set; }
        public DbSet<Orden> ordenes { get; set; }

        public DbSet<Producto> productos { get; set; }

        public DbSet<CategoriaProducto> categoriaProductos { get; set; }

        public DbSet<DetalleOrden> detallesOrden { get; set; }
    }
}
