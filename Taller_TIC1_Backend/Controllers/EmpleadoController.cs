using Microsoft.AspNetCore.Mvc;
using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Services.Interfaces;

namespace Taller_TIC1_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadoController : ControllerBase
    {
        private readonly IEmpleadoService _empleadoService;

        public EmpleadoController(IEmpleadoService empleadoService)
        {
            _empleadoService = empleadoService;
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
            var deleted = await _empleadoService.DeleteAsync(id);
            if (!deleted)
                return NotFound($"Empleado con ID {id} no encontrado");

            return NoContent();
        }
    }
}
