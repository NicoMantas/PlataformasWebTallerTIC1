using Microsoft.AspNetCore.Mvc;
using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Services.Interfaces;

namespace Taller_TIC1_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepuestoProveedorController : Controller
    {
        private readonly IRepuestoProveedorService _service;

        public RepuestoProveedorController(IRepuestoProveedorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RepuestoProveedorResponseDto>>> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("repuesto/{idRepuesto}/proveedor/{idProveedor}")]
        public async Task<ActionResult<RepuestoProveedorResponseDto>> GetById(int idRepuesto, int idProveedor)
        {
            var item = await _service.GetByIdAsync(idRepuesto, idProveedor);
            if (item is null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<RepuestoProveedorResponseDto>> Create(RepuestoProveedorCreateDto dto)
        {
            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(
                    nameof(GetById),
                    new { idRepuesto = created.IdRepuesto, idProveedor = created.IdProveedor },
                    created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpDelete("repuesto/{idRepuesto}/proveedor/{idProveedor}")]
        public async Task<IActionResult> Delete(int idRepuesto, int idProveedor)
        {
            var ok = await _service.DeleteAsync(idRepuesto, idProveedor);
            return ok ? NoContent() : NotFound();
        }

        [HttpGet("repuesto/{idRepuesto}")]
        public async Task<ActionResult<IEnumerable<RepuestoProveedorResponseDto>>> GetByRepuestoId(int idRepuesto)
        {
            var items = await _service.GetByRepuestoIdAsync(idRepuesto);
            return Ok(items);
        }

        [HttpGet("proveedor/{idProveedor}")]
        public async Task<ActionResult<IEnumerable<RepuestoProveedorResponseDto>>> GetByProveedorId(int idProveedor)
        {
            var items = await _service.GetByProveedorIdAsync(idProveedor);
            return Ok(items);
        }
    }
}
