using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using taller_backend.ContextDB;
using taller_backend.Models;

namespace taller_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoPersonaController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        public TipoPersonaController(ApplicationDbContext ctx) { _ctx = ctx; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoPersona>>> GetAll()
            => Ok(await _ctx.TipoPersona.ToListAsync());

        [HttpGet("{id:long}")]
        public async Task<ActionResult<TipoPersona>> GetById(long id)
            => await _ctx.TipoPersona.FindAsync(id) is { } e ? Ok(e) : NotFound();

    }
}
