using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using taller_backend.ContextDB;
using taller_backend.Models;
using taller_backend.DTOs;

namespace taller_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicioController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        public ServicioController(ApplicationDbContext ctx) { _ctx = ctx; }

        // GET api/Servicios?includeRefs=true
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Servicio>>> GetAll([FromQuery] bool includeRefs = false)
        {
            IQueryable<Servicio> q = _ctx.Servicios;
            if (includeRefs)
                q = q.Include(s => s.Estado)
                     .Include(s => s.TipoServicio)
                     .Include(s => s.Reparacion)
                     .Include(s => s.Revision);
            return Ok(await q.ToListAsync());
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<Servicio>> GetById(long id, [FromQuery] bool includeRefs = false)
        {
            IQueryable<Servicio> q = _ctx.Servicios;
            if (includeRefs)
                q = q.Include(s => s.Estado)
                     .Include(s => s.TipoServicio)
                     .Include(s => s.Reparacion)
                     .Include(s => s.Revision);

            var s = await q.FirstOrDefaultAsync(x => x.Id == id);
            return s is null ? NotFound() : Ok(s);
        }

        [HttpPost]
        public async Task<ActionResult<Servicio>> Create(ServicioCreateDto dto)
        {
            if (dto.IdEstado.HasValue && !await _ctx.Estados.AnyAsync(x => x.Id == dto.IdEstado.Value))
                return BadRequest($"Estado {dto.IdEstado} no existe.");
            if (dto.IdTipoServicio.HasValue && !await _ctx.TiposServicio.AnyAsync(x => x.Id == dto.IdTipoServicio.Value))
                return BadRequest($"TipoServicio {dto.IdTipoServicio} no existe.");

            var s = new Servicio
            {
                Descripcion = dto.Descripcion,
                CostoBase = dto.CostoBase,
                TiempoEstimado = dto.TiempoEstimado,
                IdEstado = dto.IdEstado,
                IdTipoServicio = dto.IdTipoServicio
            };

            _ctx.Servicios.Add(s);
            await _ctx.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = s.Id }, s);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, ServicioUpdateDto dto)
        {
            var s = await _ctx.Servicios.FindAsync(id);
            if (s == null) return NotFound();

            if (dto.IdEstado.HasValue && !await _ctx.Estados.AnyAsync(x => x.Id == dto.IdEstado.Value))
                return BadRequest($"Estado {dto.IdEstado} no existe.");
            if (dto.IdTipoServicio.HasValue && !await _ctx.TiposServicio.AnyAsync(x => x.Id == dto.IdTipoServicio.Value))
                return BadRequest($"TipoServicio {dto.IdTipoServicio} no existe.");

            s.Descripcion = dto.Descripcion;
            s.CostoBase = dto.CostoBase;
            s.TiempoEstimado = dto.TiempoEstimado;
            s.IdEstado = dto.IdEstado;
            s.IdTipoServicio = dto.IdTipoServicio;

            await _ctx.SaveChangesAsync();
            return NoContent();
        }

        // Crear Reparacion 1:1 para un Servicio
        [HttpPost("{id:long}/reparacion")]
        public async Task<ActionResult<Reparacion>> CreateReparacion(long id, ReparacionCreateDto dto)
        {
            var s = await _ctx.Servicios.FindAsync(id);
            if (s == null) return NotFound();

            var exists = await _ctx.Reparaciones.AnyAsync(r => r.IdServicio == id);
            if (exists) return Conflict("El servicio ya tiene Reparacion.");

            var r = new Reparacion { IdServicio = id, ManoDeObra = dto.ManoDeObra };
            _ctx.Reparaciones.Add(r);
            await _ctx.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id }, r);
        }

        // Crear Revision 1:1 para un Servicio
        [HttpPost("{id:long}/revision")]
        public async Task<ActionResult<Revision>> CreateRevision(long id, RevisionCreateDto dto)
        {
            var s = await _ctx.Servicios.FindAsync(id);
            if (s == null) return NotFound();

            var exists = await _ctx.Revisiones.AnyAsync(r => r.IdServicio == id);
            if (exists) return Conflict("El servicio ya tiene Revision.");

            var r = new Revision { IdServicio = id, Diagnostico = dto.Diagnostico };
            _ctx.Revisiones.Add(r);
            await _ctx.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id }, r);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var s = await _ctx.Servicios.FindAsync(id);
            if (s == null) return NotFound();

            _ctx.Servicios.Remove(s);
            await _ctx.SaveChangesAsync();
            return NoContent();
        }
    }
}
