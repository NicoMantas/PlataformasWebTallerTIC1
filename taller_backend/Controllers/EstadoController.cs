using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using taller_backend.ContextDB;
using taller_backend.DTOs;
using taller_backend.Models;

namespace taller_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadoController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        public EstadoController(ApplicationDbContext ctx) { _ctx = ctx; }

        // api para obtener todos los estados
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estado>>> GetAll()
            => Ok(await _ctx.Estados.ToListAsync());

        // api para obtener un estado por id
        [HttpGet("{id:long}")]
        public async Task<ActionResult<Estado>> GetById(long id)
            => await _ctx.Estados.FindAsync(id) is { } e ? Ok(e) : NotFound();
    }
}