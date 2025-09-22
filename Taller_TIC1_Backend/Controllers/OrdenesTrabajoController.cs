using Microsoft.AspNetCore.Mvc;
using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Services.Interfaces;

namespace Taller_TIC1_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdenesTrabajoController : Controller
    {

        private readonly IOrdenTrabajoService _ordenService;

        public OrdenesTrabajoController(IOrdenTrabajoService ordenService)
        {
            _ordenService = ordenService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrdenTrabajoDTO>>> GetOrdenes()
        {
            var ordenes = await _ordenService.GetAllOrdenesAsync();
            return Ok(ordenes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrdenTrabajoDTO>> GetOrden(int id)
        {
            var orden = await _ordenService.GetOrdenByIdAsync(id);
            if (orden == null) return NotFound();
            return Ok(orden);
        }

        [HttpPost]
        public async Task<ActionResult<OrdenTrabajoDTO>> CreateOrden(OrdenTrabajoCreateDTO ordenDto)
        {
            var orden = await _ordenService.CreateOrdenAsync(ordenDto);
            return CreatedAtAction(nameof(GetOrden), new { id = orden.Id }, orden);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrden(int id, OrdenTrabajoDTO ordenDto)
        {
            if (id != ordenDto.Id) return BadRequest();

            try
            {
                var orden = await _ordenService.UpdateOrdenAsync(id, ordenDto);
                return Ok(orden);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrden(int id)
        {
            var result = await _ordenService.DeleteOrdenAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPost("{ordenId}/servicios/{servicioId}")]
        public async Task<IActionResult> AddServicioToOrden(int ordenId, int servicioId)
        {
            var result = await _ordenService.AddServicioToOrdenAsync(ordenId, servicioId);
            if (!result) return BadRequest();
            return Ok();
        }

        [HttpDelete("{ordenId}/servicios/{servicioId}")]
        public async Task<IActionResult> RemoveServicioFromOrden(int ordenId, int servicioId)
        {
            var result = await _ordenService.RemoveServicioFromOrdenAsync(ordenId, servicioId);
            if (!result) return BadRequest();
            return Ok();
        }

    }
}
