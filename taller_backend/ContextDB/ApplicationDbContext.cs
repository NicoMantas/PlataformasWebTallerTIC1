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
        public DbSet<Estado> Estados => Set<Estado>();
        public DbSet<TipoServicio> TiposServicio => Set<TipoServicio>();
        public DbSet<Servicio> Servicios => Set<Servicio>();
        public DbSet<Reparacion> Reparaciones => Set<Reparacion>();
        public DbSet<Revision> Revisiones => Set<Revision>();
        public DbSet<Persona> Personas => Set<Persona>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<ClienteNatural> ClientesNaturales => Set<ClienteNatural>();
        public DbSet<ClienteJuridico> ClientesJuridicos => Set<ClienteJuridico>();
        public DbSet<TipoPersona> TipoPersona => Set<TipoPersona>();
        public DbSet<DetalleTipoPersona> DetallesTipoPersona => Set<DetalleTipoPersona>();
        public DbSet<Empleado> Empleados => Set<Empleado>();
        public DbSet<Mecanico> Mecanicos => Set<Mecanico>();
        public DbSet<Vehiculo> Vehiculos => Set<Vehiculo>();
        public DbSet<TipoVehiculo> TiposVehiculo => Set<TipoVehiculo>();
        public DbSet<VehiculoGasolina> VehiculosGasolina => Set<VehiculoGasolina>();
        public DbSet<VehiculoElectrico> VehiculosElectricos => Set<VehiculoElectrico>();
        public DbSet<VehiculoHibrido> VehiculosHibridos => Set<VehiculoHibrido>();
        public DbSet<Taller> Talleres => Set<Taller>();
        public DbSet<OrdenDeTrabajo> OrdenesDeTrabajo => Set<OrdenDeTrabajo>();
        public DbSet<OrdenServicio> OrdenServicios => Set<OrdenServicio>();
        public DbSet<TipoOrdenEstado> TipoOrdenEstado => Set<TipoOrdenEstado>();
        public DbSet<TallerEmpleado> TallerEmpleados => Set<TallerEmpleado>();
        public DbSet<TallerCliente> TallerClientes => Set<TallerCliente>();
        public DbSet<TallerVehiculo> TallerVehiculos => Set<TallerVehiculo>();
        public DbSet<InventarioTaller> InventariosTaller => Set<InventarioTaller>();
        public DbSet<ServicioMecanico> ServiciosMecanicos => Set<ServicioMecanico>();

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

            // Persona
            modelBuilder.Entity<Persona>(e =>
            {
                e.ToTable("Persona");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id").UseIdentityByDefaultColumn();
                e.Property(x => x.Nombre).HasColumnName("nombre");
                e.Property(x => x.Email).HasColumnName("email");
                e.HasIndex(x => x.Email).IsUnique(); // según script
                e.Property(x => x.Telefono).HasColumnName("telefono");
                e.Property(x => x.IdTDetalleTipoPersona).HasColumnName("idTDetalleTipoPersona");
                e.Property(x => x.FechaCreacion).HasColumnName("fechaCreacion");
            });

            // Usuarios
            modelBuilder.Entity<Usuario>(e =>
            {
                e.ToTable("Usuarios");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id").UseIdentityByDefaultColumn();
                e.Property(x => x.IdPersona).HasColumnName("idPersona");
                e.Property(x => x.Username).HasColumnName("username");
                e.HasIndex(x => x.Username).IsUnique();
                e.Property(x => x.PasswordHash).HasColumnName("passwordHash");
                e.Property(x => x.Activo).HasColumnName("activo");
                e.Property(x => x.FechaCreacion).HasColumnName("fechaCreacion");
                e.Property(x => x.UltimoLogin).HasColumnName("ultimoLogin");

                e.HasOne(x => x.Persona)
                 .WithOne(x => x.Usuario)
                 .HasForeignKey<Usuario>(x => x.IdPersona)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ClientesNaturales
            modelBuilder.Entity<ClienteNatural>(e =>
            {
                e.ToTable("clientesNaturales");
                e.HasKey(x => x.IdPersona); // PK = FK
                e.Property(x => x.IdPersona).HasColumnName("idPersona");
                e.Property(x => x.Cedula).HasColumnName("cedula");
                e.HasIndex(x => x.Cedula).IsUnique();
                e.Property(x => x.Apellido).HasColumnName("apellido");

                e.HasOne(x => x.Persona)
                 .WithOne(x => x.ClienteNatural)
                 .HasForeignKey<ClienteNatural>(x => x.IdPersona)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ClientesJuridicos
            modelBuilder.Entity<ClienteJuridico>(e =>
            {
                e.ToTable("clientesJuridicos");
                e.HasKey(x => x.IdPersona); // PK = FK
                e.Property(x => x.IdPersona).HasColumnName("idPersona");
                e.Property(x => x.Nit).HasColumnName("nit");
                e.HasIndex(x => x.Nit).IsUnique();
                e.Property(x => x.RepresentanteLegal).HasColumnName("representanteLegal");

                e.HasOne(x => x.Persona)
                 .WithOne(x => x.ClienteJuridico)
                 .HasForeignKey<ClienteJuridico>(x => x.IdPersona)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // TipoPersona
            modelBuilder.Entity<TipoPersona>(e =>
            {
                e.ToTable("TipoPersona");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id").UseIdentityByDefaultColumn();
                e.Property(x => x.Descripcion).HasColumnName("descripcion");
            });

            // DetalleTipoPersona
            modelBuilder.Entity<DetalleTipoPersona>(e =>
            {
                e.ToTable("DetalleTipoPersona");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id").UseIdentityByDefaultColumn();
                e.Property(x => x.Descripcion).HasColumnName("descripcion");
                e.Property(x => x.IdTipoPersona).HasColumnName("idTipoPersona");

                e.HasOne<TipoPersona>()
                 .WithMany()
                 .HasForeignKey(x => x.IdTipoPersona)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Empleados
            modelBuilder.Entity<Empleado>(e =>
            {
                e.ToTable("Empleados");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id").UseIdentityByDefaultColumn();
                e.Property(x => x.Nombre).HasColumnName("nombre");
                e.Property(x => x.Apellido).HasColumnName("apellido");
                e.Property(x => x.Cedula).HasColumnName("cedula");
                e.HasIndex(x => x.Cedula).IsUnique();
                e.Property(x => x.Salario).HasColumnName("salario");
                e.Property(x => x.FechaContratacion).HasColumnName("fechaContratacion");
                e.Property(x => x.IdDetalleTipoPersona).HasColumnName("idDetalleTipoPersona");

                e.HasOne<DetalleTipoPersona>()
                 .WithMany()
                 .HasForeignKey(x => x.IdDetalleTipoPersona)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Mecanico)
                 .WithOne(x => x.Empleado)
                 .HasForeignKey<Mecanico>(x => x.IdEmpleado);
            });

            // Mecanico
            modelBuilder.Entity<Mecanico>(e =>
            {
                e.ToTable("Mecanico");
                e.HasKey(x => x.IdEmpleado); // PK = FK
                e.Property(x => x.IdEmpleado).HasColumnName("idEmpleado");
                e.Property(x => x.Especialidad).HasColumnName("especialidad");
                e.Property(x => x.TareasTrabajadas).HasColumnName("tareasTrabajadas");
            });

            // Mecanico
            modelBuilder.Entity<Mecanico>(e =>
            {
                e.ToTable("Mecanico");
                e.HasKey(x => x.IdEmpleado); // PK = FK
                e.Property(x => x.IdEmpleado).HasColumnName("idEmpleado");
                e.Property(x => x.Especialidad).HasColumnName("especialidad");
                e.Property(x => x.TareasTrabajadas).HasColumnName("tareasTrabajadas");
            });

            // Claves compuestas y mapeos mínimos
            modelBuilder.Entity<OrdenServicio>(e =>
            {
                e.ToTable("OrdenServicio");
                e.HasKey(x => new { x.IdOrden, x.IdServicio });
            });

            modelBuilder.Entity<TallerEmpleado>(e =>
            {
                e.ToTable("TallerEmpleado");
                e.HasKey(x => new { x.IdTaller, x.IdEmpleado });
            });

            modelBuilder.Entity<TallerCliente>(e =>
            {
                e.ToTable("TallerCliente");
                e.HasKey(x => new { x.IdTaller, x.IdCliente });
            });

            modelBuilder.Entity<TallerVehiculo>(e =>
            {
                e.ToTable("TallerVehiculo");
                e.HasKey(x => new { x.IdTaller, x.PlacaVehiculo });
            });

            modelBuilder.Entity<InventarioTaller>(e =>
            {
                e.ToTable("InventarioTaller");
                e.HasKey(x => new { x.IdTaller, x.IdRepuesto });
                e.Property(x => x.IdTaller).HasColumnName("idTaller");
                e.Property(x => x.IdRepuesto).HasColumnName("idRepuesto");
                e.Property(x => x.Cantidad).HasColumnName("cantidad");
                e.Property(x => x.StockMinimo).HasColumnName("stockMinimo");

                e.HasOne(x => x.Taller)
                 .WithMany(t => t.InventarioTaller)
                 .HasForeignKey(x => x.IdTaller)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Repuesto)
                 .WithMany()
                 .HasForeignKey(x => x.IdRepuesto)
                 .OnDelete(DeleteBehavior.Restrict);
            });

             // Claves compuestas y mapeos mínimos
            modelBuilder.Entity<OrdenServicio>(e =>
            {
                e.ToTable("OrdenServicio");
                e.HasKey(x => new { x.IdOrden, x.IdServicio });
            });

            modelBuilder.Entity<TallerEmpleado>(e =>
            {
                e.ToTable("TallerEmpleado");
                e.HasKey(x => new { x.IdTaller, x.IdEmpleado });
            });

            modelBuilder.Entity<TallerCliente>(e =>
            {
                e.ToTable("TallerCliente");
                e.HasKey(x => new { x.IdTaller, x.IdCliente });
            });

            modelBuilder.Entity<TipoOrdenEstado>(e =>
            {
                e.ToTable("TipoOrdenEstado");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id").UseIdentityByDefaultColumn();
                e.Property(x => x.Descripcion).HasColumnName("descripcion");
            });

            modelBuilder.Entity<InventarioTaller>(e =>
            {
                e.ToTable("InventarioTaller");
                e.HasKey(x => new { x.IdTaller, x.IdRepuesto });
                e.Property(x => x.IdTaller).HasColumnName("idTaller");
                e.Property(x => x.IdRepuesto).HasColumnName("idRepuesto");
                e.Property(x => x.Cantidad).HasColumnName("cantidad");
                e.Property(x => x.StockMinimo).HasColumnName("stockMinimo");

                e.HasOne(x => x.Taller)
                 .WithMany(t => t.InventarioTaller)
                 .HasForeignKey(x => x.IdTaller)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Repuesto)
                 .WithMany()
                 .HasForeignKey(x => x.IdRepuesto)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Vehiculo y derivados
            modelBuilder.Entity<Vehiculo>(e =>
            {
                e.ToTable("Vehiculo");
                e.HasKey(x => x.Placa);
                e.Property(x => x.Placa).HasColumnName("placa");
                e.Property(x => x.Marca).HasColumnName("marca");
                e.Property(x => x.Modelo).HasColumnName("modelo");
                e.Property(x => x.Año).HasColumnName("año");
                e.Property(x => x.IdPersona).HasColumnName("idPersona");
                e.Property(x => x.IdTipoVehiculo).HasColumnName("idTipoVehiculo");
                e.Property(x => x.FechaCreacion).HasColumnName("fechaCreacion");

                e.HasOne(x => x.Persona)
                 .WithMany()
                 .HasForeignKey(x => x.IdPersona)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.TipoVehiculo)
                 .WithMany(tv => tv.Vehiculos)
                 .HasForeignKey(x => x.IdTipoVehiculo)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TipoVehiculo>(e =>
            {
                e.ToTable("TipoVehiculo");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id").UseIdentityByDefaultColumn();
                e.Property(x => x.Descripcion).HasColumnName("descripcion");
            });

            modelBuilder.Entity<VehiculoGasolina>(e =>
            {
                e.ToTable("VehiculoGasolina");
                e.HasKey(x => x.Placa);
                e.Property(x => x.Placa).HasColumnName("placa");
                e.Property(x => x.Cilindraje).HasColumnName("cilindraje");
                e.HasOne(x => x.Vehiculo)
                 .WithOne()
                 .HasForeignKey<VehiculoGasolina>(x => x.Placa)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<VehiculoElectrico>(e =>
            {
                e.ToTable("VehiculoElectrico");
                e.HasKey(x => x.Placa);
                e.Property(x => x.Placa).HasColumnName("placa");
                e.Property(x => x.CapacidadBateria).HasColumnName("capacidadBateria");
                e.HasOne(x => x.Vehiculo)
                 .WithOne()
                 .HasForeignKey<VehiculoElectrico>(x => x.Placa)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<VehiculoHibrido>(e =>
            {
                e.ToTable("VehiculoHibrido");
                e.HasKey(x => x.Placa);
                e.Property(x => x.Placa).HasColumnName("placa");
                e.Property(x => x.Cilindraje).HasColumnName("cilindraje");
                e.Property(x => x.CapacidadBateria).HasColumnName("capacidadBateria");
                e.HasOne(x => x.Vehiculo)
                 .WithOne()
                 .HasForeignKey<VehiculoHibrido>(x => x.Placa)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ServicioMecanicos (tabla puente)
            modelBuilder.Entity<ServicioMecanico>(e =>
            {
                e.ToTable("ServicioMecanico");
                e.HasKey(x => new { x.IdServicio, x.IdMecanico });
                e.Property(x => x.IdServicio).HasColumnName("idServicio");
                e.Property(x => x.IdMecanico).HasColumnName("idMecanico");
                e.Property(x => x.FechaAsignacion)
                 .HasColumnName("fechaAsignacion")
                 .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.HasOne(x => x.Servicio)
                 .WithMany()
                 .HasForeignKey(x => x.IdServicio)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Mecanico)
                 .WithMany()
                 .HasForeignKey(x => x.IdMecanico)
                 .OnDelete(DeleteBehavior.Restrict);
            });


        }
    }
}
