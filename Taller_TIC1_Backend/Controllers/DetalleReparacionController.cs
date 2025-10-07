using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Taller_TIC1_Backend.Data;
using Taller_TIC1_Backend.Models;

namespace Taller_TIC1_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetalleReparacionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DetalleReparacionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/DetalleReparacion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetalleReparacion>>> GetReparaciones()
        {
            var items = await _context.DetallesReparacion
                .Include(dr => dr.Servicio)
                .Include(dr => dr.DetalleReparacionRepuesto)
                .ToListAsync();
            return Ok(items);
        }

        // GET: api/DetalleReparacion/5  (idServicio)
        [HttpGet("{id:int}")]
        public async Task<ActionResult<DetalleReparacion>> GetReparacion(int id)
        {
            var entity = await _context.DetallesReparacion
                .Include(dr => dr.Servicio)
                .Include(dr => dr.DetalleReparacionRepuesto)
                .FirstOrDefaultAsync(x => x.IdServicio == id);
            if (entity == null) return NotFound();
            return Ok(entity);
        }

        // POST: api/DetalleReparacion
        [HttpPost]
        public async Task<ActionResult<DetalleReparacion>> PostReparacion(DetalleReparacion dto)
        {
            // Validar que el servicio exista
            var servicioExists = await _context.Servicios.AnyAsync(s => s.Id == dto.IdServicio);
            if (!servicioExists) return BadRequest("El servicio especificado no existe.");

            // Validar que no exista ya una reparación para ese servicio (1:1)
            var exists = await _context.DetallesReparacion.AnyAsync(r => r.IdServicio == dto.IdServicio);
            if (exists) return BadRequest("Ya existe una reparación para este servicio.");

            _context.DetallesReparacion.Add(dto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReparacion), new { id = dto.IdServicio }, dto);
        }

        // PUT: api/DetalleReparacion/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutReparacion(int id, DetalleReparacion dto)
        {
            if (id != dto.IdServicio) return BadRequest();

            var entity = await _context.DetallesReparacion.FindAsync(id);
            if (entity == null) return NotFound();

            entity.IdDetalleReparacionRepuesto = dto.IdDetalleReparacionRepuesto;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/DetalleReparacion/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteReparacion(int id)
        {
            var entity = await _context.DetallesReparacion.FindAsync(id);
            if (entity == null) return NotFound();

            _context.DetallesReparacion.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/DetalleReparacion/{id}/repuestos
        [HttpGet("{id:int}/repuestos")]
        public async Task<ActionResult<IEnumerable<DetalleReparacionRepuesto>>> GetRepuestosByReparacion(int id)
        {
            var detalleReparacion = await _context.DetallesReparacion
                .Include(dr => dr.DetalleReparacionRepuesto)
                .FirstOrDefaultAsync(dr => dr.IdServicio == id);
            
            if (detalleReparacion == null) return NotFound();

            var repuestos = await _context.DetallesReparacionRepuesto
                .Where(drr => drr.Id == detalleReparacion.IdDetalleReparacionRepuesto)
                .Include(drr => drr.Repuesto)
                .ToListAsync();

            return Ok(repuestos);
        }

        // POST: api/DetalleReparacion/{id}/repuestos
        [HttpPost("{id:int}/repuestos")]
        public async Task<ActionResult<DetalleReparacionRepuesto>> AddRepuestoToReparacion(int id, [FromBody] AddRepuestoDto dto)
        {
            var detalleReparacion = await _context.DetallesReparacion
                .FirstOrDefaultAsync(dr => dr.IdServicio == id);
            
            if (detalleReparacion == null) return NotFound();

            // Crear o encontrar el contenedor de repuestos
            var detalleReparacionRepuesto = new DetalleReparacionRepuesto
            {
                IdRepuesto = dto.IdRepuesto,
                Cantidad = dto.Cantidad
            };

            _context.DetallesReparacionRepuesto.Add(detalleReparacionRepuesto);
            await _context.SaveChangesAsync();

            // Actualizar la referencia en DetalleReparacion si es necesario
            if (detalleReparacion.IdDetalleReparacionRepuesto == 0)
            {
                detalleReparacion.IdDetalleReparacionRepuesto = detalleReparacionRepuesto.Id;
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetRepuestosByReparacion), new { id }, detalleReparacionRepuesto);
        }
    }
}

public class AddRepuestoDto
{
    public int IdRepuesto { get; set; }
    public int Cantidad { get; set; }
}