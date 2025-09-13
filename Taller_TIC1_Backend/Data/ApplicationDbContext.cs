using B_TallerAutomoviles.Clases;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Taller_TIC1_Backend.Models;



namespace Taller_TIC1_Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        //acá van los DbSet para cada entidad
        public DbSet<Proveedor> Proveedores { get; set; } //estos DbSet son como tablas en la base de datos y vienen de las clases que creamos en la carpeta Model
        public DbSet<Repuesto> Repuestos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<OrdenDeTrabajo> OrdenesDeTrabajo { get; set; }
        public DbSet<Reparacion> Reparaciones { get; set; }
        public DbSet<Revision> Revisiones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configuraciones adicionales si es necesario

            // Configuración para Proveedor
            modelBuilder.Entity<Proveedor>(e =>
            {
                e.ToTable("Proveedor");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
                e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
                e.Property(x => x.Contacto).HasColumnName("contacto").HasMaxLength(100);
            });

            modelBuilder.Entity<Repuesto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Numero_serie).HasColumnType("int8");
                entity.Property(e => e.Precio).HasColumnType("float4");
            });

        }
    }
}
