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
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Agregar despu�s de builder.Services.AddControllers();
            builder.Services.AddScoped<IServicioRepository, ServicioRepository>();
            builder.Services.AddScoped<IOrdenTrabajoRepository, OrdenTrabajoRepository>();
            builder.Services.AddScoped<IServicioService, ServicioService>();
            builder.Services.AddScoped<IOrdenTrabajoService, OrdenTrabajoService>();

            // Agregar AutoMapper
            builder.Services.AddAutoMapper(cfg => {
                cfg.AddProfile<Data.MappingProfile>();
            });

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

            // DI Factura
            builder.Services.AddScoped<IFacturaRepository, FacturaRepository>();
            builder.Services.AddScoped<IFacturaService, FacturaService>();

            // DI Cliente
            builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
            builder.Services.AddScoped<IClienteService, ClienteService>();

            // DI Vehiculo
            builder.Services.AddScoped<IVehiculoRepository, VehiculoRepository>();
            builder.Services.AddScoped<IVehiculoService, VehiculoService>();

            // DI Empleado
            builder.Services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
            builder.Services.AddScoped<IEmpleadoService, EmpleadoService>();

            // Agrega estas l�neas para registrar los nuevos servicios
            builder.Services.AddScoped<IRepuestoProveedorRepository, RepuestoProveedorRepository>();
            builder.Services.AddScoped<IRepuestoProveedorService, RepuestoProveedorService>();

            // Registrar repositorios
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<ITallerRepository, TallerRepository>();
            builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
            builder.Services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
            builder.Services.AddScoped<IVehiculoRepository, VehiculoRepository>();

            // Registrar servicios
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITallerService, TallerService>();
            builder.Services.AddScoped<IClienteService, ClienteService>();
            builder.Services.AddScoped<IEmpleadoService, EmpleadoService>();
            builder.Services.AddScoped<IVehiculoService, VehiculoService>();

            // Configurar CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("ReactPolicy", policy =>
                {
                    policy.WithOrigins("http://localhost:3000", "http://localhost:5173", "https://localhost:5173", "http://localhost:3000", "https://localhost:3000") // URLs de tu aplicación React
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Usar CORS - debe estar antes de UseAuthorization y MapControllers
            app.UseCors("ReactPolicy");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
