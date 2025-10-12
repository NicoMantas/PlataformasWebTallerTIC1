using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Taller_TIC1_Backend.Data;
using Taller_TIC1_Backend.Models;
using Taller_TIC1_Backend.Models.DTOs;
using AutoMapper;

namespace Taller_TIC1_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetalleRevisionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DetalleRevisionController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/DetalleRevision
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetalleRevisionDTO>>> GetDetallesRevision()
        {
            var items = await _context.DetallesRevision.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<DetalleRevisionDTO>>(items));
        }

        // GET: api/DetalleRevision/5 (idServicio)
        [HttpGet("{id:int}")]
        public async Task<ActionResult<DetalleRevisionDTO>> GetDetalleRevision(int id)
        {
            var entity = await _context.DetallesRevision
                .FirstOrDefaultAsync(x => x.IdServicio == id);
            
            if (entity == null) return NotFound();
            
            return Ok(_mapper.Map<DetalleRevisionDTO>(entity));
        }

        // POST: api/DetalleRevision
        [HttpPost]
        public async Task<ActionResult<DetalleRevisionDTO>> PostDetalleRevision(DetalleRevisionCreateDTO dto)
        {
            // Validar que el servicio exista
            var servicioExists = await _context.Servicios.AnyAsync(s => s.Id == dto.IdServicio);
            if (!servicioExists) return BadRequest("El servicio especificado no existe.");

            // Validar que no exista ya un detalle de revisión para ese servicio (1:1)
            var exists = await _context.DetallesRevision.AnyAsync(r => r.IdServicio == dto.IdServicio);
            if (exists) return BadRequest("Ya existe un detalle de revisión para este servicio.");

            var entity = _mapper.Map<DetalleRevision>(dto);
            _context.DetallesRevision.Add(entity);
            await _context.SaveChangesAsync();

            var createdDto = _mapper.Map<DetalleRevisionDTO>(entity);
            return CreatedAtAction(nameof(GetDetalleRevision), new { id = dto.IdServicio }, createdDto);
        }

        // PUT: api/DetalleRevision/5 (idServicio)
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutDetalleRevision(int id, DetalleRevisionUpdateDTO dto)
        {
            if (id != dto.IdServicio) return BadRequest();

            var entity = await _context.DetallesRevision
                .FirstOrDefaultAsync(x => x.IdServicio == id);
            
            if (entity == null) return NotFound();

            // Actualizar solo los campos que pueden cambiar
            entity.Detalles = dto.Detalles;
            entity.DetallesEncontrados = dto.DetallesEncontrados;
            
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/DetalleRevision/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDetalleRevision(int id)
        {
            var entity = await _context.DetallesRevision
                .FirstOrDefaultAsync(x => x.IdServicio == id);
            
            if (entity == null) return NotFound();

            _context.DetallesRevision.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
