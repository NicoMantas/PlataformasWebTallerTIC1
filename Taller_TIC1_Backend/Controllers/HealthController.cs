using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Taller_TIC1_Backend.Data;

namespace Taller_TIC1_Backend.Controllers
{
    [ApiController]
    [Route("api / [controller]")]
    public class HealthController : Controller
    {
        private readonly ApplicationDbContext _db;
        public HealthController(ApplicationDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> CheckDatabaseConnection()
        {
            try
            {
                // Realiza una consulta simple para verificar la conexión (puedes cambiar "Pigmentos" por otra tabla)
                return Ok(new
                {
                    message = "Conexión exitosa con Supabase",
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al conectar con Supabase",
                    error = ex.Message
                });
            }
        }
    }
}
