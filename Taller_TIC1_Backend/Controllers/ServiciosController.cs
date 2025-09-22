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
    }
}
