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
        public DbSet<taller_backend.Models.Estado> Estados => Set<taller_backend.Models.Estado>();
        public DbSet<taller_backend.Models.TipoServicio> TiposServicio => Set<taller_backend.Models.TipoServicio>();
        public DbSet<taller_backend.Models.Servicio> Servicios => Set<taller_backend.Models.Servicio>();
        public DbSet<taller_backend.Models.Reparacion> Reparaciones => Set<taller_backend.Models.Reparacion>();
        public DbSet<taller_backend.Models.Revision> Revisiones => Set<taller_backend.Models.Revision>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración para la entidad Proveedor
            modelBuilder.Entity<Proveedor>(e =>
            {
                e.ToTable("Proveedor");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id").UseIdentityByDefaultColumn();
                e.Property(x => x.Nombre).HasColumnName("nombre");
                e.Property(x => x.Contacto).HasColumnName("contacto");

                e.HasMany(x => x.Repuestos)
                 .WithOne(x => x.Proveedor)
                 .HasForeignKey(x => x.IdProveedor)
                 .OnDelete(DeleteBehavior.SetNull);
            });
            
            // Configuración para la entidad Repuesto
            modelBuilder.Entity<Repuesto>(e =>
            {
                e.ToTable("Repuesto");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id").UseIdentityByDefaultColumn();
                e.Property(x => x.Nombre).HasColumnName("nombre");
                e.Property(x => x.NumeroParte).HasColumnName("numeroParte");
                e.Property(x => x.Descripcion).HasColumnName("descripcion");
                e.Property(x => x.CostoCompra).HasColumnName("costoCompra");
                e.Property(x => x.PrecioVenta).HasColumnName("precioVenta");
                e.Property(x => x.CantidadStock).HasColumnName("cantidadStock");
                e.Property(x => x.IdProveedor).HasColumnName("idProveedor");
            });

            // Configuración para la entidad Estado
            modelBuilder.Entity<Models.Estado>(e =>
            {
                e.ToTable("Estado");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id").UseIdentityByDefaultColumn();
                e.Property(x => x.Descripcion).HasColumnName("descripcion");
            });

            // Configuración para la entidad TipoServicio
            modelBuilder.Entity<Models.TipoServicio>(e =>
            {
                e.ToTable("TipoServicio");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id").UseIdentityByDefaultColumn();
                e.Property(x => x.Descripcion).HasColumnName("descripcion");
            });

            // Configuración para la entidad Servicio
            modelBuilder.Entity<Models.Servicio>(e =>
            {
                e.ToTable("Servicio");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id").UseIdentityByDefaultColumn();

                e.Property(x => x.Descripcion).HasColumnName("descripcion");
                e.Property(x => x.CostoBase).HasColumnName("costoBase");
                e.Property(x => x.TiempoEstimado).HasColumnName("tiempoEstimado");
                e.Property(x => x.IdEstado).HasColumnName("idEstado");
                e.Property(x => x.IdTipoServicio).HasColumnName("idTipoServicio");

                e.HasOne(x => x.Estado)
                 .WithMany()
                 .HasForeignKey(x => x.IdEstado)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.TipoServicio)
                 .WithMany()
                 .HasForeignKey(x => x.IdTipoServicio)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Reparacion)
                 .WithOne(x => x.Servicio)
                 .HasForeignKey<Models.Reparacion>(x => x.IdServicio);

                e.HasOne(x => x.Revision)
                 .WithOne(x => x.Servicio)
                 .HasForeignKey<Models.Revision>(x => x.IdServicio);
            });

            // Configuración para la entidad Reparacion
            modelBuilder.Entity<Models.Reparacion>(e =>
            {
                e.ToTable("Reparacion");
                e.HasKey(x => x.IdServicio);
                e.Property(x => x.IdServicio).HasColumnName("idServicio"); // PK = FK
                e.Property(x => x.ManoDeObra).HasColumnName("mano_de_obra");
            });

            // Configuración para la entidad Revision
            modelBuilder.Entity<Models.Revision>(e =>
            {
                e.ToTable("Revision");
                e.HasKey(x => x.IdServicio);
                e.Property(x => x.IdServicio).HasColumnName("idServicio");
                e.Property(x => x.Diagnostico).HasColumnName("diagnostico");
            });

        }
    }
}
