using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using taller_backend.ContextDB;
using taller_backend.DTOs;
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

        [HttpPost]
        public async Task<ActionResult<TipoVehiculo>> Create(TipoVehiculoCreateDto dto)
        {
            var e = new TipoVehiculo { Descripcion = dto.Descripcion };
            _ctx.TiposVehiculo.Add(e);
            await _ctx.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = e.Id }, e);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, TipoVehiculoUpdateDto dto)
        {
            var e = await _ctx.TiposVehiculo.FindAsync(id);
            if (e == null) return NotFound();
            e.Descripcion = dto.Descripcion;
            await _ctx.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var e = await _ctx.TiposVehiculo.FindAsync(id);
            if (e == null) return NotFound();
            _ctx.TiposVehiculo.Remove(e);
            await _ctx.SaveChangesAsync();
            return NoContent();
        }
    }
}