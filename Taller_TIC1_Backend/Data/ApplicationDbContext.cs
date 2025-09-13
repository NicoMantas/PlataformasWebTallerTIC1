using Microsoft.EntityFrameworkCore;
using Taller_TIC1_Backend.Models;

namespace Taller_TIC1_Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets para todas las entidades
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Repuesto> Repuestos { get; set; }
        public DbSet<RepuestoProveedor> RepuestoProveedores { get; set; }
        public DbSet<TipoEmpleado> TiposEmpleado { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<EstadoServicio> EstadosServicio { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }
        public DbSet<VGasolina> VehiculosGasolina { get; set; }
        public DbSet<VElectrico> VehiculosElectricos { get; set; }
        public DbSet<VHibrido> VehiculosHibridos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<CNatural> ClientesNaturales { get; set; }
        public DbSet<CEmpresa> ClientesEmpresa { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<DetalleRevision> DetallesRevision { get; set; }
        public DbSet<DetalleReparacionRepuesto> DetallesReparacionRepuesto { get; set; }
        public DbSet<DetalleReparacion> DetallesReparacion { get; set; }
        public DbSet<Taller> Talleres { get; set; }
        public DbSet<UsuarioEmpleadoTaller> UsuariosEmpleadoTaller { get; set; }
        public DbSet<UsuarioClienteTaller> UsuariosClienteTaller { get; set; }
        public DbSet<TipoEstadoOrden> TiposEstadoOrden { get; set; }
        public DbSet<OrdenDeTrabajo> OrdenesDeTrabajo { get; set; }
        public DbSet<DetalleServicioOrden> DetallesServicioOrden { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 

            // Configuración para Proveedor
            modelBuilder.Entity<Proveedor>(e =>
            {
                e.ToTable("Proveedor");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id").UseIdentityColumn();
                e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(255).IsRequired();
                e.Property(x => x.Contacto).HasColumnName("contacto").HasMaxLength(255);
            });

            // Configuración para Repuesto
            modelBuilder.Entity<Repuesto>(e =>
            {
                e.ToTable("Repuesto");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id").UseIdentityColumn();
                e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(255).IsRequired();
                e.Property(x => x.Numero_serie).HasColumnName("numero_serie").HasColumnType("bigint");
                e.Property(x => x.Precio).HasColumnName("precio").HasColumnType("real");
                e.Property(x => x.Stock).HasColumnName("stock").HasDefaultValue(0);
            });

            // Configuración para RepuestoProveedor (tabla intermedia)
            modelBuilder.Entity<RepuestoProveedor>(e =>
            {
                e.ToTable("RepuestoProveedor");
                e.HasKey(x => new { x.IdRepuesto, x.IdProveedor });
                e.Property(x => x.IdRepuesto).HasColumnName("idRepuesto");
                e.Property(x => x.IdProveedor).HasColumnName("idProveedor");

                // Configuración de las relaciones con navegación
                e.HasOne(rp => rp.Repuesto)
                 .WithMany()
                 .HasForeignKey(x => x.IdRepuesto)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(rp => rp.Proveedor)
                 .WithMany()
                 .HasForeignKey(x => x.IdProveedor)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración para TipoEmpleado
            modelBuilder.Entity<TipoEmpleado>(e =>
            {
                e.ToTable("TipoEmpleado");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id").UseIdentityColumn();
                e.Property(x => x.Descripcion).HasColumnName("descripcion").HasMaxLength(255);
            });

            // Configuración para Empleado
            modelBuilder.Entity<Empleado>(e =>
            {
                e.ToTable("Empleado");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id").UseIdentityColumn();
                e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(255);
                e.Property(x => x.Apellido).HasColumnName("apellido").HasMaxLength(255);
                e.Property(x => x.Cedula).HasColumnName("cedula").HasColumnType("bigint");
                e.Property(x => x.Salario).HasColumnName("salario").HasColumnType("double precision");
                e.Property(x => x.FechaContratacion).HasColumnName("fechaContratacion").HasColumnType("date");
                e.Property(x => x.IdTipoEmpleado).HasColumnName("idTipoEmpleado");

                e.HasOne<TipoEmpleado>()
                 .WithMany()
                 .HasForeignKey(x => x.IdTipoEmpleado)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración para EstadoServicio
            modelBuilder.Entity<EstadoServicio>(e =>
            {
                e.ToTable("EstadoServicio");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id").UseIdentityColumn();
                e.Property(x => x.Descripcion).HasColumnName("descripcion").HasMaxLength(255);
            });

            // Configuración para Vehiculo
            // Configuración para Vehiculo
            modelBuilder.Entity<Vehiculo>(e =>
            {
                e.ToTable("Vehiculo");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.Placa).HasColumnName("placa").HasMaxLength(50);
                e.Property(x => x.Marca).HasColumnName("marca").HasMaxLength(50);
                e.Property(x => x.Modelo).HasColumnName("modelo").HasMaxLength(50);
                e.Property(x => x.Anio).HasColumnName("anio");
            });

            // Configuración para VGasolina con relación explícita
            modelBuilder.Entity<VGasolina>(e =>
            {
                e.ToTable("VGasolina");
                e.HasKey(x => x.IdVehiculo);
                e.Property(x => x.IdVehiculo).HasColumnName("idVehiculo");
                e.Property(x => x.Cilindraje).HasColumnName("cilindraje");

                // Relación uno a uno EXPLÍCITA
                e.HasOne(vg => vg.Vehiculo)
                 .WithOne(v => v.VGasolina)
                 .HasForeignKey<VGasolina>(vg => vg.IdVehiculo)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración para VElectrico con relación explícita
            modelBuilder.Entity<VElectrico>(e =>
            {
                e.ToTable("VElectrico");
                e.HasKey(x => x.IdVehiculo);
                e.Property(x => x.IdVehiculo).HasColumnName("idVehiculo");
                e.Property(x => x.CapacidadBateria).HasColumnName("capacidadBateria");

                // Relación uno a uno EXPLÍCITA
                e.HasOne(ve => ve.Vehiculo)
                 .WithOne(v => v.VElectrico)
                 .HasForeignKey<VElectrico>(ve => ve.IdVehiculo)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración para VHibrido con relación explícita
            modelBuilder.Entity<VHibrido>(e =>
            {
                e.ToTable("VHibrido");
                e.HasKey(x => x.IdVehiculo);
                e.Property(x => x.IdVehiculo).HasColumnName("idVehiculo");
                e.Property(x => x.CapacidadBateria).HasColumnName("capacidadBateria");
                e.Property(x => x.Cilindraje).HasColumnName("cilindraje");

                // Relación uno a uno EXPLÍCITA
                e.HasOne(vh => vh.Vehiculo)
                 .WithOne(v => v.VHibrido)
                 .HasForeignKey<VHibrido>(vh => vh.IdVehiculo)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración para Cliente
            modelBuilder.Entity<Cliente>(e =>
            {
                e.ToTable("Cliente");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(255);
                e.Property(x => x.Email).HasColumnName("email").HasMaxLength(255);
                e.Property(x => x.Telefono).HasColumnName("telefono").HasColumnType("bigint");
                e.Property(x => x.IdVehiculo).HasColumnName("idVehiculo");


                e.HasOne(c => c.Vehiculo)
                 .WithMany()
                 .HasForeignKey(x => x.IdVehiculo)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración para CNatural
            modelBuilder.Entity<CNatural>(e =>
            {
                e.ToTable("CNatural");
                e.HasKey(x => x.IdCliente);
                e.Property(x => x.IdCliente).HasColumnName("idCliente");
                e.Property(x => x.Cedula).HasColumnName("cedula").HasColumnType("bigint");
                e.Property(x => x.Apellido).HasColumnName("apellido").HasMaxLength(255);

                e.HasOne(c => c.Cliente)
                 .WithOne(c => c.CNatural)
                 .HasForeignKey<CNatural>(x => x.IdCliente)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración para CEmpresa
            modelBuilder.Entity<CEmpresa>(e =>
            {
                e.ToTable("CEmpresa");
                e.HasKey(x => x.IdCliente);
                e.Property(x => x.IdCliente).HasColumnName("idCliente");
                e.Property(x => x.Nit).HasColumnName("nit").HasColumnType("bigint");
                e.Property(x => x.RepresentanteLegal).HasColumnName("representante").HasMaxLength(255);

                e.HasOne(c => c.Cliente)
                 .WithOne(c => c.CEmpresa)
                 .HasForeignKey<CEmpresa>(x => x.IdCliente)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración para Servicio
            modelBuilder.Entity<Servicio>(e =>
            {
                e.ToTable("Servicio");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id").UseIdentityColumn();
                e.Property(x => x.IdCliente).HasColumnName("idCliente");
                e.Property(x => x.IdEmpleado).HasColumnName("idEmpleado");
                e.Property(x => x.IdEstado).HasColumnName("idEstado");
                e.Property(x => x.Costo).HasColumnName("costo").HasColumnType("real");

                e.HasOne<Cliente>()
                 .WithMany()
                 .HasForeignKey(x => x.IdCliente)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne<Empleado>()
                 .WithMany()
                 .HasForeignKey(x => x.IdEmpleado)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne<EstadoServicio>()
                 .WithMany()
                 .HasForeignKey(x => x.IdEstado)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración para DetalleRevision
            modelBuilder.Entity<DetalleRevision>(e =>
            {
                e.ToTable("DetalleRevision");
                e.HasKey(x => x.IdServicio);
                e.Property(x => x.IdServicio).HasColumnName("idServicio");
                e.Property(x => x.Detalles).HasColumnName("detalles").HasMaxLength(255);

                e.HasOne<Servicio>()
                 .WithOne()
                 .HasForeignKey<DetalleRevision>(x => x.IdServicio)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración para DetalleReparacionRepuesto
            modelBuilder.Entity<DetalleReparacionRepuesto>(e =>
            {
                e.ToTable("DetalleReparacionRepuesto");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id").UseIdentityColumn();
                e.Property(x => x.IdRepuesto).HasColumnName("idRepuesto");
                e.Property(x => x.Cantidad).HasColumnName("cantidad");

                e.HasOne<Repuesto>()
                 .WithMany()
                 .HasForeignKey(x => x.IdRepuesto)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración para DetalleReparacion
            modelBuilder.Entity<DetalleReparacion>(e =>
            {
                e.ToTable("DetalleReparacion");
                e.HasKey(x => x.IdServicio);
                e.Property(x => x.IdServicio).HasColumnName("idServicio");
                e.Property(x => x.IdDetalleReparacionRepuesto).HasColumnName("idDetalleReparacionRepuesto");

                e.HasOne<Servicio>()
                 .WithOne()
                 .HasForeignKey<DetalleReparacion>(x => x.IdServicio)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne<DetalleReparacionRepuesto>()
                 .WithMany()
                 .HasForeignKey(x => x.IdDetalleReparacionRepuesto)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración para Taller
            modelBuilder.Entity<Taller>(e =>
            {
                e.ToTable("Taller");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id").UseIdentityColumn();
                e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(255);
                e.Property(x => x.Direccion).HasColumnName("direccion").HasMaxLength(255);
            });

            // Configuración para UsuarioEmpleadoTaller
            modelBuilder.Entity<UsuarioEmpleadoTaller>(e =>
            {
                e.ToTable("UsuariosEmpleadoTaller");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id").UseIdentityColumn();
                e.Property(x => x.Email).HasColumnName("email").HasMaxLength(255);
                e.Property(x => x.Contrasena).HasColumnName("contrasena").HasMaxLength(255);
                e.Property(x => x.IdTaller).HasColumnName("idTaller");
                e.Property(x => x.IdEmpleado).HasColumnName("idEmpleado");
                // Relaciones CORRECTAS
                e.HasOne(u => u.Taller)
                 .WithMany(t => t.UsuariosEmpleados)
                 .HasForeignKey(u => u.IdTaller)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(u => u.Empleado)
                 .WithMany()
                 .HasForeignKey(u => u.IdEmpleado)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración para UsuarioClienteTaller
            modelBuilder.Entity<UsuarioClienteTaller>(e =>
            {
                e.ToTable("UsuariosClienteTaller");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id").UseIdentityColumn();
                e.Property(x => x.Email).HasColumnName("email").HasMaxLength(255);
                e.Property(x => x.Contrasena).HasColumnName("contrasena").HasMaxLength(255);
                e.Property(x => x.IdTaller).HasColumnName("idTaller");
                e.Property(x => x.IdCliente).HasColumnName("idCliente");

                // Relaciones CORRECTAS
                e.HasOne(u => u.Taller)
                 .WithMany(t => t.UsuariosClientes)
                 .HasForeignKey(u => u.IdTaller)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(u => u.Cliente)
                 .WithMany()
                 .HasForeignKey(u => u.IdCliente)
                 .OnDelete(DeleteBehavior.Restrict);

                // Configuración para TipoEstadoOrden
                modelBuilder.Entity<TipoEstadoOrden>(e =>
            {
                e.ToTable("TipoEstadoOrden");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id").UseIdentityColumn();
                e.Property(x => x.Descripcion).HasColumnName("descripcion").HasMaxLength(255);
            });

                // Configuración para OrdenDeTrabajo
                modelBuilder.Entity<OrdenDeTrabajo>(e =>
                {
                    e.ToTable("OrdenTrabajo");
                    e.HasKey(x => x.Id);
                    e.Property(x => x.Id).HasColumnName("id").UseIdentityColumn();
                    e.Property(x => x.FechaCreacion).HasColumnName("fechaCreacion").HasColumnType("date");
                    e.Property(x => x.IdTipoEstadoOrden).HasColumnName("idTipoEstadoOrden");

                    e.HasOne<TipoEstadoOrden>()
                     .WithMany()
                     .HasForeignKey(x => x.IdTipoEstadoOrden)
                     .OnDelete(DeleteBehavior.Restrict);
                });

                // Configuración para DetalleServicioOrden
                modelBuilder.Entity<DetalleServicioOrden>(e =>
                {
                    e.ToTable("DetalleServicioOrden");
                    e.HasKey(x => new { x.IdOrdenTrabajo, x.IdServicio });
                    e.Property(x => x.IdOrdenTrabajo).HasColumnName("idOrdenTrabajo");
                    e.Property(x => x.IdServicio).HasColumnName("idServicio");

                    e.HasOne<OrdenDeTrabajo>()
                     .WithMany()
                     .HasForeignKey(x => x.IdOrdenTrabajo)
                     .OnDelete(DeleteBehavior.Cascade);

                    e.HasOne<Servicio>()
                     .WithMany()
                     .HasForeignKey(x => x.IdServicio)
                     .OnDelete(DeleteBehavior.Cascade);
                });
            });
        }
    }
}
