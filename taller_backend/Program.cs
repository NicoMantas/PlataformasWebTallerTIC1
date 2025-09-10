
using Microsoft.EntityFrameworkCore;
using Supabase;
using taller_backend.ContextDB;

namespace taller_backend
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuración de servicios
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configurar Entity Framework con PostgreSQL
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Configurar cliente Supabase
            var supabaseUrl = builder.Configuration["Supabase:Url"]
                ?? throw new InvalidOperationException("Supabase:Url no está configurado");
            var supabaseKey = builder.Configuration["Supabase:Key"]
                ?? throw new InvalidOperationException("Supabase:Key no está configurado");

            builder.Services.AddSingleton(provider => new Client(supabaseUrl, supabaseKey, new SupabaseOptions
            {
                AutoConnectRealtime = false,
                AutoRefreshToken = true
            }));

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

            // Inicialización asíncrona de Supabase (sin bloquear el hilo principal)
            _ = Task.Run(async () =>
            {
                try
                {
                    var supabaseClient = app.Services.GetRequiredService<Client>();
                    await supabaseClient.InitializeAsync();
                    app.Logger.LogInformation("✅ Cliente Supabase inicializado correctamente");
                }
                catch (Exception ex)
                {
                    app.Logger.LogError(ex, "❌ Error inicializando Supabase");
                    // No hacemos throw para que la aplicación pueda continuar
                }
            });
            //compilador se queja por no usar await
            //app.Run();
            await app.RunAsync();
        }
    }
}