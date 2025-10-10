using Microsoft.AspNetCore.Mvc;
using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Services.Interfaces;
using Taller_TIC1_Backend.Services;
using System.Security.Claims;

namespace Taller_TIC1_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadoController : ControllerBase
    {
        private readonly IEmpleadoService _empleadoService;
        private readonly AuthorizationService _authorizationService;

        public EmpleadoController(IEmpleadoService empleadoService, AuthorizationService authorizationService)
        {
            _empleadoService = empleadoService;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpleadoResponseDto>>> GetAll()
        {
            var empleados = await _empleadoService.GetAllAsync();
            return Ok(empleados);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmpleadoResponseDto>> GetById(int id)
        {
            var empleado = await _empleadoService.GetByIdAsync(id);
            if (empleado == null)
                return NotFound($"Empleado con ID {id} no encontrado");

            return Ok(empleado);
        }

        [HttpGet("cedula/{cedula}")]
        public async Task<ActionResult<EmpleadoResponseDto>> GetByCedula(long cedula)
        {
            var empleado = await _empleadoService.GetByCedulaAsync(cedula);
            if (empleado == null)
                return NotFound($"Empleado con cédula {cedula} no encontrado");

            return Ok(empleado);
        }

        [HttpPost]
        public async Task<ActionResult<EmpleadoResponseDto>> Create(EmpleadoCreateDto empleadoCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var empleado = await _empleadoService.CreateAsync(empleadoCreateDto);
                return CreatedAtAction(nameof(GetById), new { id = empleado.Id }, empleado);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear el empleado: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EmpleadoResponseDto>> Update(int id, EmpleadoUpdateDto empleadoUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var empleado = await _empleadoService.UpdateAsync(id, empleadoUpdateDto);
            if (empleado == null)
                return NotFound($"Empleado con ID {id} no encontrado");

            return Ok(empleado);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            // TODO: Implementar autenticación cuando esté disponible
            // Por ahora permitimos la operación para testing
            // var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            // if (string.IsNullOrEmpty(userEmail) || !await _authorizationService.IsAdminAsync(userEmail))
            // {
            //     return Forbid("Solo los administradores pueden desactivar empleados");
            // }

            var deleted = await _empleadoService.DeleteAsync(id);
            if (!deleted)
                return NotFound($"Empleado con ID {id} no encontrado");

            return Ok(new { message = "Empleado desactivado exitosamente" });
        }

        [HttpPost("{id}/desactivar")]
        public async Task<ActionResult> DesactivarConDetalles(int id, EmpleadoDesactivarDto desactivarDto, [FromQuery] int? adminId = null)
        {
            // Verificación básica de autorización usando parámetro de query
            if (adminId.HasValue)
            {
                var isAdmin = await _authorizationService.IsAdminAsync(adminId.Value);
                if (!isAdmin)
                {
                    return BadRequest(new { message = "Solo los administradores pueden desactivar empleados" });
                }
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var desactivado = await _empleadoService.DesactivarConDetallesAsync(id, desactivarDto.DetallesDesactivacion);
            if (!desactivado)
                return NotFound($"Empleado con ID {id} no encontrado");

            return Ok(new { message = "Empleado desactivado exitosamente con detalles registrados" });
        }

        [HttpPatch("{id}/activate")]
        public async Task<ActionResult> Activate(int id, [FromQuery] int? adminId = null)
        {
            // Verificación básica de autorización usando parámetro de query
            if (adminId.HasValue)
            {
                var isAdmin = await _authorizationService.IsAdminAsync(adminId.Value);
                if (!isAdmin)
                {
                    return BadRequest(new { message = "Solo los administradores pueden reactivar empleados" });
                }
            }

            var activated = await _empleadoService.ActivateAsync(id);
            if (!activated)
                return NotFound($"Empleado con ID {id} no encontrado");

            return Ok(new { message = "Empleado activado exitosamente" });
        }
    }
}
