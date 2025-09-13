using Microsoft.AspNetCore.Mvc;
using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Services.Interfaces;
using static Taller_TIC1_Backend.Models.DTOs.PorveedorCreateDTOcs;

namespace Taller_TIC1_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedorController : ControllerBase
    {
        private readonly IProveedorService _proveedorService;

        public ProveedorController(IProveedorService proveedorService)
        {
            _proveedorService = proveedorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProveedorResponseDto>>> GetAll()
        {
            var proveedores = await _proveedorService.GetAllAsync();
            return Ok(proveedores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProveedorResponseDto>> GetById(int id)
        {
            var proveedor = await _proveedorService.GetByIdAsync(id);
            if (proveedor == null)
                return NotFound();

            return Ok(proveedor);
        }

        [HttpPost]
        public async Task<ActionResult<ProveedorResponseDto>> Create(ProveedorCreateDto dto)
        {
            var proveedor = await _proveedorService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = proveedor.Id }, proveedor);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProveedorResponseDto>> Update(int id, ProveedorUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var proveedor = await _proveedorService.UpdateAsync(dto);
            if (proveedor == null)
                return NotFound();

            return Ok(proveedor);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _proveedorService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ProveedorResponseDto>>> SearchByName([FromQuery] string name)
        {
            var proveedores = await _proveedorService.SearchByNameAsync(name);
            return Ok(proveedores);
        }

    }
}
