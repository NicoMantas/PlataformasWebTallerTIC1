using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using taller_backend.ContextDB;
using taller_backend.Models;

namespace taller_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoVehiculoController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        public TipoVehiculoController(ApplicationDbContext ctx) { _ctx = ctx; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoVehiculo>>> GetAll()
            => Ok(await _ctx.TiposVehiculo.ToListAsync());

        [HttpGet("{id:long}")]
        public async Task<ActionResult<TipoVehiculo>> GetById(long id)
            => await _ctx.TiposVehiculo.FindAsync(id) is { } e ? Ok(e) : NotFound();
    }
}