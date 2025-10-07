using Microsoft.AspNetCore.Mvc;
using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Services.Interfaces;

namespace Taller_TIC1_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiciosController : Controller
    {
        private readonly IServicioService _servicioService;

        public ServiciosController(IServicioService servicioService)
        {
            _servicioService = servicioService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicioDTO>>> GetServicios()
        {
            var servicios = await _servicioService.GetAllServiciosAsync();
            return Ok(servicios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServicioDTO>> GetServicio(int id)
        {
            var servicio = await _servicioService.GetServicioByIdAsync(id);
            if (servicio == null) return NotFound();
            return Ok(servicio);
        }

        [HttpPost]
        public async Task<ActionResult<ServicioDTO>> CreateServicio(ServicioCreateDTO servicioDto)
        {
            var servicio = await _servicioService.CreateServicioAsync(servicioDto);
            return CreatedAtAction(nameof(GetServicio), new { id = servicio.Id }, servicio);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServicio(int id, ServicioDTO servicioDto)
        {
            if (id != servicioDto.Id) return BadRequest();

            try
            {
                var servicio = await _servicioService.UpdateServicioAsync(id, servicioDto);
                return Ok(servicio);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServicio(int id)
        {
            var result = await _servicioService.DeleteServicioAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        // Listar servicios por cliente
        [HttpGet("por-cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<ServicioDTO>>> GetByCliente(int clienteId)
        {
            var items = await _servicioService.GetServiciosByClienteAsync(clienteId);
            return Ok(items);
        }

        // Activos: Pendiente/En proceso
        [HttpGet("by-cliente/{clienteId}/activos")]
        public async Task<ActionResult<IEnumerable<ServicioDTO>>> GetActivosByCliente(int clienteId)
        {
            var items = await _servicioService.GetServiciosActivosByClienteAsync(clienteId);
            return Ok(items);
        }

        // Historial: Completado/Cancelado
        [HttpGet("by-cliente/{clienteId}/historial")]
        public async Task<ActionResult<IEnumerable<ServicioDTO>>> GetHistorialByCliente(int clienteId)
        {
            var items = await _servicioService.GetServiciosHistorialByClienteAsync(clienteId);
            return Ok(items);
        }

        // Cancelar servicio
        [HttpPost("{id}/cancelar")]
        public async Task<IActionResult> CancelarServicio(int id)
        {
            var ok = await _servicioService.CancelarServicioAsync(id);
            if (!ok) return NotFound();
            return Ok();
        }

        // Secretaría: listar pendientes (activos sin mecánico)
        [HttpGet("secretaria/pendientes")]
        public async Task<ActionResult<IEnumerable<ServicioDTO>>> SecretariaPendientes()
        {
            var items = await _servicioService.SecretariaListPendientesAsync();
            return Ok(items);
        }

        // Secretaría: listar asignados (activos con mecánico)
        [HttpGet("secretaria/asignados")]
        public async Task<ActionResult<IEnumerable<ServicioDTO>>> SecretariaAsignados()
        {
            var items = await _servicioService.SecretariaListAsignadosAsync();
            return Ok(items);
        }

        // Secretaría: asignar mecánico
        [HttpPost("secretaria/{servicioId}/asignar/{empleadoId}")]
        public async Task<IActionResult> SecretariaAsignar(int servicioId, int empleadoId)
        {
            var ok = await _servicioService.SecretariaAsignarMecanicoAsync(servicioId, empleadoId);
            if (!ok) return NotFound();
            return Ok();
        }
    }
}
