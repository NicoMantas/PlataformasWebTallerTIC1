using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using taller_backend.ContextDB;
using taller_backend.Models;
using taller_backend.DTOs;
namespace taller_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoServicioController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        public TipoServicioController(ApplicationDbContext ctx) { _ctx = ctx; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoServicio>>> GetAll()
            => Ok(await _ctx.TiposServicio.ToListAsync());

        [HttpGet("{id:long}")]
        public async Task<ActionResult<TipoServicio>> GetById(long id)
            => await _ctx.TiposServicio.FindAsync(id) is { } e ? Ok(e) : NotFound();

        [HttpPost]
        public async Task<ActionResult<TipoServicio>> Create(TipoServicioCreateDto dto)
        {
            var e = new TipoServicio { Descripcion = dto.Descripcion };
            _ctx.TiposServicio.Add(e);
            await _ctx.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = e.Id }, e);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, TipoServicioUpdateDto dto)
        {
            var e = await _ctx.TiposServicio.FindAsync(id);
            if (e == null) return NotFound();
            e.Descripcion = dto.Descripcion;
            await _ctx.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var e = await _ctx.TiposServicio.FindAsync(id);
            if (e == null) return NotFound();
            _ctx.TiposServicio.Remove(e);
            await _ctx.SaveChangesAsync();
            return NoContent();
        }
    }
}
