using Microsoft.AspNetCore.Mvc;
using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Services.Interfaces;

namespace Taller_TIC1_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TallerController : Controller
    {
        private readonly ITallerService _tallerService; // Servicio para manejar la lógica de negocio relacionada con Taller
        public TallerController(ITallerService tallerService) // Inyección de dependencia del servicio
        {
            _tallerService = tallerService;
        }

        // Endpoint para obtener el taller
        [HttpGet]
        public async Task<ActionResult<TallerResponseDto>> GetAll()
        {
            try
            {
                var talleres = await _tallerService.GetAllAsync();
                return Ok(talleres);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener los talleres: {ex.Message}");
            }

        }
        // Endpoint para obtener un taller por su ID
        [HttpGet("{id}")]
        public async Task<ActionResult<TallerResponseDto>> GetById(int id)
        {
            try
            {
                var taller = await _tallerService.GetByIdAsync(id);
                if (taller == null)
                    return NotFound($"Taller con ID {id} no encontrado");

                return Ok(taller);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el taller: {ex.Message}");
            }
        }
        // Endpoint para crear un nuevo taller
        [HttpPost]
        public async Task<ActionResult<TallerResponseDto>> Create(TallerCreateDto dto)
        {
            try
            {
                var taller = await _tallerService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = taller.Id }, taller);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear el taller: {ex.Message}");
            }
        }
        // Endpoint para actulizar el name de un taller
        [HttpPut("{id}")] 
        public async Task<ActionResult<TallerResponseDto>> Update(int id, TallerUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest("El ID del taller no coincide con el ID de la ruta");
            try
            {
                var exists = await _tallerService.ExistsAsync(id);
                if (!exists)
                    return NotFound($"Taller con ID {id} no encontrado");
                var updatedTaller = await _tallerService.UpdateAsync(id, dto);
                return Ok(updatedTaller);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar el taller: {ex.Message}");
            }
        }

        // Endpoint para eliminar un taller por su ID
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await _tallerService.DeleteAsync(id);
                if (!result)
                    return NotFound($"Taller con ID {id} no encontrado");
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar el taller: {ex.Message}");
            }
        }

    }
}
