using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taller_TIC1_Backend.Data;
using Taller_TIC1_Backend.Models;
using Taller_TIC1_Backend.Models.DTOs;

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
        public async Task<ActionResult<IEnumerable<DetalleReparacion>>> GetDetallesReparacion()
        {
            var items = await _context.DetallesReparacion
                .Include(dr => dr.Servicio)
                .Include(dr => dr.DetalleReparacionRepuesto)
                    .ThenInclude(drr => drr.Repuesto)
                .ToListAsync();

            return Ok(items);
        }

        // GET: api/DetalleReparacion/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<DetalleReparacion>> GetDetalleReparacion(int id)
        {
            var detalleReparacion = await _context.DetallesReparacion
                .Include(dr => dr.Servicio)
                .Include(dr => dr.DetalleReparacionRepuesto)
                    .ThenInclude(drr => drr.Repuesto)
                .FirstOrDefaultAsync(dr => dr.IdServicio == id);

            if (detalleReparacion == null)
            {
                return NotFound();
            }

            return Ok(detalleReparacion);
        }

        // POST: api/DetalleReparacion
        [HttpPost]
        public async Task<ActionResult<DetalleReparacion>> PostDetalleReparacion(DetalleReparacionCreateDTO dto)
        {
            var servicioExists = await _context.Servicios.AnyAsync(s => s.Id == dto.IdServicio);
            if (!servicioExists) return BadRequest("El servicio especificado no existe.");

            var detalleRepuestoExists = await _context.DetallesReparacionRepuesto.AnyAsync(drr => drr.Id == dto.IdDetalleReparacionRepuesto);
            if (!detalleRepuestoExists) return BadRequest("El detalle de repuesto especificado no existe.");

            var existingDetail = await _context.DetallesReparacion.FindAsync(dto.IdServicio);
            if (existingDetail != null) return BadRequest("Ya existe un detalle de reparación para este servicio.");

            var entity = new DetalleReparacion
            {
                IdServicio = dto.IdServicio,
                IdDetalleReparacionRepuesto = dto.IdDetalleReparacionRepuesto
            };

            _context.DetallesReparacion.Add(entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDetalleReparacion), new { id = entity.IdServicio }, entity);
        }
        // PUT: api/DetalleReparacion/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutDetalleReparacion(int id, DetalleReparacionUpdateDto dto)
        {
            if (id != dto.IdServicio) return BadRequest();

            var entity = await _context.DetallesReparacion.FindAsync(id);
            if (entity == null) return NotFound();

            var detalleRepuestoExists = await _context.DetallesReparacionRepuesto.AnyAsync(drr => drr.Id == dto.IdDetalleReparacionRepuesto);
            if (!detalleRepuestoExists) return BadRequest("El detalle de repuesto especificado no existe.");

            entity.IdDetalleReparacionRepuesto = dto.IdDetalleReparacionRepuesto;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/DetalleReparacion/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDetalleReparacion(int id)
        {
            var detalleReparacion = await _context.DetallesReparacion.FindAsync(id);
            if (detalleReparacion == null)
            {
                return NotFound();
            }

            _context.DetallesReparacion.Remove(detalleReparacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DetalleReparacionExists(int id)
        {
            return _context.DetallesReparacion.Any(e => e.IdServicio == id);
        }
    }
}