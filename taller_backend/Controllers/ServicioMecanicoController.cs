/*using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using taller_backend.ContextDB;
using taller_backend.DTOs;
using taller_backend.Models;

namespace taller_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicioMecanicoController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        public ServicioMecanicoController(ApplicationDbContext ctx) { _ctx = ctx; }

        // GET api/ServicioMecanico?idServicio=1&idMecanico=2
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicioMecanico>>> Get([FromQuery] long? idServicio, [FromQuery] long? idMecanico)
        {
            IQueryable<ServicioMecanico> q = _ctx.ServicioMecanico;
            if (idServicio.HasValue) q = q.Where(x => x.IdServicio == idServicio.Value);
            if (idMecanico.HasValue) q = q.Where(x => x.IdMecanico == idMecanico.Value);
            return Ok(await q.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult<ServicioMecanico>> Create(ServicioMecanicoCreateDto dto)
        {
            if (!await _ctx.Servicios.AnyAsync(s => s.Id == dto.IdServicio))
                return BadRequest($"Servicio {dto.IdServicio} no existe.");
            if (!await _ctx.Mecanicos.AnyAsync(m => m.IdEmpleado == dto.IdMecanico))
                return BadRequest($"Mecanico {dto.IdMecanico} no existe.");

            var exists = await _ctx.ServicioMecanico.AnyAsync(sm => sm.IdServicio == dto.IdServicio && sm.IdMecanico == dto.IdMecanico);
            if (exists) return Conflict("Ya existe la asignación.");

            var sm = new ServicioMecanico
            {
                IdServicio = dto.IdServicio,
                IdMecanico = dto.IdMecanico
            };
            _ctx.ServicioMecanicos.Add(sm);
            await _ctx.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { idServicio = sm.IdServicio, idMecanico = sm.IdMecanico }, sm);
        }

        [HttpDelete("{idServicio:long}/{idMecanico:long}")]
        public async Task<IActionResult> Delete(long idServicio, long idMecanico)
        {
            var sm = await _ctx.ServicioMecanico.FirstOrDefaultAsync(x => x.IdServicio == idServicio && x.IdMecanico == idMecanico);
            if (sm == null) return NotFound();
            _ctx.ServicioMecanico.Remove(sm);
            await _ctx.SaveChangesAsync();
            return NoContent();
        }
    }
} */
