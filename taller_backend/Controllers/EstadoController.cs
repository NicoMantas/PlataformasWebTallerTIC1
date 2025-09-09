using Microsoft.AspNetCore.Mvc;
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

        //api para obtener todos los estados
    /*    [HttpGet]
        public async Task<ActionResult<IEnumerable<Estado>>> GetAll()
            => Ok(await _ctx.Estados.ToListAsync());*/

        //api para obtener un estado por id
        [HttpGet("{id:long}")]
        public async Task<ActionResult<Estado>> GetById(long id)
            => await _ctx.Estados.FindAsync(id) is { } e ? Ok(e) : NotFound();

        //api para crear un estado
        [HttpPost]
        public async Task<ActionResult<Estado>> Create(EstadoCreateDto dto)
        {
            var e = new Estado { Descripcion = dto.Descripcion };
            _ctx.Estados.Add(e);
            await _ctx.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = e.Id }, e);
        }

        //api para actualizar un estado
        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, EstadoUpdateDto dto)
        {
            var e = await _ctx.Estados.FindAsync(id);
            if (e == null) return NotFound();
            e.Descripcion = dto.Descripcion;
            await _ctx.SaveChangesAsync();
            return NoContent();
        }

        //api para eliminar un estado
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var e = await _ctx.Estados.FindAsync(id);
            if (e == null) return NotFound();
            _ctx.Estados.Remove(e);
            await _ctx.SaveChangesAsync();
            return NoContent();
        }
    }
}

