using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Taller_TIC1_Backend.Data;
using Taller_TIC1_Backend.Models;

namespace Taller_TIC1_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetalleRevisionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DetalleRevisionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/DetalleRevision
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetalleRevision>>> GetRevisiones()
        {
            // Trae revisiones con su servicio
            var items = await _context.DetallesRevision
                .Include(dr => dr.IdServicio)
                .ToListAsync();
            return Ok(items);
        }

        // GET: api/DetalleRevision/5  (idServicio)
        [HttpGet("{id:int}")]
        public async Task<ActionResult<DetalleRevision>> GetRevision(int id)
        {
            var entity = await _context.DetallesRevision.FirstOrDefaultAsync(x => x.IdServicio == id);
            if (entity == null) return NotFound();
            return Ok(entity);
        }

        // POST: api/DetalleRevision
        [HttpPost]
        public async Task<ActionResult<DetalleRevision>> PostRevision(DetalleRevision dto)
        {
            // Validar que el servicio exista
            var servicioExists = await _context.Servicios.AnyAsync(s => s.Id == dto.IdServicio);
            if (!servicioExists) return BadRequest("El servicio especificado no existe.");

            // Validar que no exista ya una revisión para ese servicio (1:1)
            var exists = await _context.DetallesRevision.AnyAsync(r => r.IdServicio == dto.IdServicio);
            if (exists) return BadRequest("Ya existe una revisión para este servicio.");

            _context.DetallesRevision.Add(dto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRevision), new { id = dto.IdServicio }, dto);
        }

        // PUT: api/DetalleRevision/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutRevision(int id, DetalleRevision dto)
        {
            if (id != dto.IdServicio) return BadRequest();

            var entity = await _context.DetallesRevision.FindAsync(id);
            if (entity == null) return NotFound();

            entity.Detalles = dto.Detalles;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/DetalleRevision/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteRevision(int id)
        {
            var entity = await _context.DetallesRevision.FindAsync(id);
            if (entity == null) return NotFound();

            _context.DetallesRevision.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}