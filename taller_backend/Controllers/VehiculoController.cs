using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using taller_backend.ContextDB;
using taller_backend.DTOs;
using taller_backend.Models;

namespace taller_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiculoController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        public VehiculoController(ApplicationDbContext ctx) { _ctx = ctx; }

        // GET api/Vehiculo?includeRefs=true
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehiculo>>> GetAll([FromQuery] bool includeRefs = false)
        {
            IQueryable<Vehiculo> q = _ctx.Vehiculos;
            if (includeRefs) q = q.Include(v => v.Persona).Include(v => v.TipoVehiculo);
            return Ok(await q.ToListAsync());
        }

        // GET api/Vehiculo/ABC123?includeRefs=true
        [HttpGet("{placa}")]
        public async Task<ActionResult<Vehiculo>> GetByPlaca(string placa, [FromQuery] bool includeRefs = false)
        {
            IQueryable<Vehiculo> q = _ctx.Vehiculos;
            if (includeRefs) q = q.Include(v => v.Persona).Include(v => v.TipoVehiculo);
            var v = await q.FirstOrDefaultAsync(x => x.Placa == placa);
            return v is null ? NotFound() : Ok(v);
        }

        // GET api/Vehiculo/by-persona/10
        [HttpGet("by-persona/{personaId:long}")]
        public async Task<ActionResult<IEnumerable<Vehiculo>>> GetByPersona(long personaId)
            => Ok(await _ctx.Vehiculos.Where(v => v.IdPersona == personaId).ToListAsync());

        [HttpPost]
        public async Task<ActionResult<Vehiculo>> Create(VehiculoCreateDto dto)
        {
            if (!string.Equals(dto.Placa, dto.Placa?.Trim(), StringComparison.Ordinal))
                dto = dto with { Placa = dto.Placa.Trim() };

            if (await _ctx.Vehiculos.AnyAsync(v => v.Placa == dto.Placa))
                return Conflict("Ya existe un vehículo con esa placa.");

            if (dto.IdPersona.HasValue && !await _ctx.Personas.AnyAsync(p => p.Id == dto.IdPersona.Value))
                return BadRequest($"Persona {dto.IdPersona} no existe.");
            if (dto.IdTipoVehiculo.HasValue && !await _ctx.TiposVehiculo.AnyAsync(t => t.Id == dto.IdTipoVehiculo.Value))
                return BadRequest($"TipoVehiculo {dto.IdTipoVehiculo} no existe.");

            var v = new Vehiculo
            {
                Placa = dto.Placa,
                Marca = dto.Marca,
                Modelo = dto.Modelo,
                Año = dto.Año,
                IdPersona = dto.IdPersona,
                IdTipoVehiculo = dto.IdTipoVehiculo
            };
            _ctx.Vehiculos.Add(v);
            await _ctx.SaveChangesAsync();
            return CreatedAtAction(nameof(GetByPlaca), new { placa = v.Placa }, v);
        }

        [HttpPut("{placa}")]
        public async Task<IActionResult> Update(string placa, VehiculoUpdateDto dto)
        {
            var v = await _ctx.Vehiculos.FindAsync(placa);
            if (v == null) return NotFound();

            if (dto.IdPersona.HasValue && !await _ctx.Personas.AnyAsync(p => p.Id == dto.IdPersona.Value))
                return BadRequest($"Persona {dto.IdPersona} no existe.");
            if (dto.IdTipoVehiculo.HasValue && !await _ctx.TiposVehiculo.AnyAsync(t => t.Id == dto.IdTipoVehiculo.Value))
                return BadRequest($"TipoVehiculo {dto.IdTipoVehiculo} no existe.");

            v.Marca = dto.Marca;
            v.Modelo = dto.Modelo;
            v.Año = dto.Año;
            v.IdPersona = dto.IdPersona;
            v.IdTipoVehiculo = dto.IdTipoVehiculo;

            await _ctx.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{placa}")]
        public async Task<IActionResult> Delete(string placa)
        {
            var v = await _ctx.Vehiculos.FindAsync(placa);
            if (v == null) return NotFound();
            _ctx.Vehiculos.Remove(v);
            await _ctx.SaveChangesAsync();
            return NoContent();
        }
    }
}