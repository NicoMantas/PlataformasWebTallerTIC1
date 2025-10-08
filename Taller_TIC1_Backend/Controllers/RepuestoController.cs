using Microsoft.AspNetCore.Mvc;
using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Services.Interfaces;

namespace Taller_TIC1_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepuestoController : ControllerBase
    {
        private readonly IRepuestoService _service;

        public RepuestoController(IRepuestoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RepuestoResponseDto>>> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RepuestoResponseDto>> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item is null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<RepuestoResponseDto>> Create(RepuestoCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, RepuestoUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var updated = await _service.UpdateAsync(dto);
            if (updated is null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<RepuestoResponseDto>>> SearchByName([FromQuery] string name)
        {
            var items = await _service.SearchByNameAsync(name);
            return Ok(items);
        }

        [HttpGet("stock/{minStock:int}")]
        public async Task<ActionResult<IEnumerable<RepuestoResponseDto>>> GetByStock(int minStock)
        {
            var items = await _service.GetByStockAsync(minStock);
            return Ok(items);
        }

        [HttpGet("serie/{numeroSerie:long}")]
        public async Task<ActionResult<RepuestoResponseDto>> GetByNumeroSerie(long numeroSerie)
        {
            var item = await _service.GetByNumeroSerieAsync(numeroSerie);
            if (item is null) return NotFound();
            return Ok(item);
        }
    }
}

