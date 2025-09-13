using B_TallerAutomoviles.Clases;
using Microsoft.EntityFrameworkCore;
using Taller_TIC1_Backend.Data;
using Taller_TIC1_Backend.Repositories;
using Taller_TIC1_Backend.Repositories.Interfaces;
using Taller_TIC1_Backend.Services;
using Taller_TIC1_Backend.Services.Interfaces;
namespace Taller_TIC1_Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // DbContext (PostgreSQL)
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
                    npg => npg.EnableRetryOnFailure()));

            // DI Proveedor
            builder.Services.AddScoped<IProveedorRepository, ProveedorRepository>();
            builder.Services.AddScoped<IProveedorService, ProveedorService>();

            // DI Repuesto
            builder.Services.AddScoped<IRepuestoRepository, RepuestoRepository>();
            builder.Services.AddScoped<IRepuestoService, RepuestoService>();

            // DI Cliente
            builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
            builder.Services.AddScoped<IClienteService, ClienteService>();

            // DI Vehiculo
            builder.Services.AddScoped<IVehiculoRepository, VehiculoRepository>();
            builder.Services.AddScoped<IVehiculoService, VehiculoService>();

            // DI Empleado
            builder.Services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
            builder.Services.AddScoped<IEmpleadoService, EmpleadoService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
