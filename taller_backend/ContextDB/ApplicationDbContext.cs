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
        public DbSet<Repuesto> Repuestos => Set<Repuesto>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Las configuraciones de modelos se agregarán aquí
            modelBuilder.Entity<Proveedor>(e =>
            {
                e.ToTable("Proveedor");
                e.HasKey(x => x.Id);
                e.Property(x => x.Nombre);
                e.Property(x => x.Contacto);
            });

            modelBuilder.Entity<Proveedor>(e =>
            {
                e.ToTable("Proveedor");
                e.HasKey(x => x.Id);

                e.HasMany(x => x.Repuestos)
                 .WithOne(x => x.Proveedor)
                 .HasForeignKey(x => x.IdProveedor)
                 .OnDelete(DeleteBehavior.SetNull); // ajusta a tu DDL: SetNull/Restrict/Cascade
            });

            modelBuilder.Entity<Repuesto>(e =>
            {
                e.ToTable("Repuesto");
                e.HasKey(x => x.Id);
            });
        }
    }
}
