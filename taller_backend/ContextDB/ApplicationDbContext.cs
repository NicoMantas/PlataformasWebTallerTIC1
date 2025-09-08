using Microsoft.EntityFrameworkCore;
using taller_backend.Models;

namespace taller_backend.ContextDB
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // acá se agregan los bdset de las tablas
        public DbSet<Proveedor> Proveedores => Set<Proveedor>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Las configuraciones de modelos se agregarán después
        }
    }
}
