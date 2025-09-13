using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using taller_backend.ContextDB;
using taller_backend.Models;
namespace taller_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoOrdenEstadoController : Controller

    {

        private readonly ApplicationDbContext _ctx;
        public TipoOrdenEstadoController(ApplicationDbContext ctx) { _ctx = ctx; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoOrdenEstado>>> GetAll()
            => Ok(await _ctx.TipoOrdenEstado.ToListAsync());

        [HttpGet("{id:long}")]
        public async Task<ActionResult<TipoOrdenEstado>> GetById(long id)
            => await _ctx.TipoOrdenEstado.FindAsync(id) is { } e ? Ok(e) : NotFound();

    }
}
