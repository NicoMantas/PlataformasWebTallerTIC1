using Microsoft.AspNetCore.Mvc;
using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Services.Interfaces;
using System.Linq;

namespace Taller_TIC1_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiculoController : ControllerBase
    {
        private readonly IVehiculoService _vehiculoService;

        public VehiculoController(IVehiculoService vehiculoService)
        {
            _vehiculoService = vehiculoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehiculoResponseDto>>> GetAll()
        {
            var vehiculos = await _vehiculoService.GetAllAsync();
            return Ok(vehiculos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehiculoResponseDto>> GetById(int id)
        {
            var vehiculo = await _vehiculoService.GetByIdAsync(id);
            if (vehiculo == null)
                return NotFound($"Vehículo con ID {id} no encontrado");

            return Ok(vehiculo);
        }

        [HttpGet("placa/{placa}")]
        public async Task<ActionResult<VehiculoResponseDto>> GetByPlaca(string placa)
        {
            var vehiculo = await _vehiculoService.GetByPlacaAsync(placa);
            if (vehiculo == null)
                return NotFound($"Vehículo con placa {placa} no encontrado");

            return Ok(vehiculo);
        }

        [HttpPost]
        public async Task<ActionResult<VehiculoResponseDto>> Create(VehiculoCreateDto vehiculoCreateDto)
        {
            // Logging temporal para debug
            Console.WriteLine($"=== DEBUG VEHICULO CREATE ===");
            Console.WriteLine($"Placa: {vehiculoCreateDto.Placa}");
            Console.WriteLine($"Marca: {vehiculoCreateDto.Marca}");
            Console.WriteLine($"Modelo: {vehiculoCreateDto.Modelo}");
            Console.WriteLine($"Anio: {vehiculoCreateDto.Anio}");
            Console.WriteLine($"IdCliente: {vehiculoCreateDto.IdCliente}");
            Console.WriteLine($"Tipo: {vehiculoCreateDto.Tipo}");
            Console.WriteLine($"Cilindraje: {vehiculoCreateDto.Cilindraje}");
            Console.WriteLine($"CapacidadBateria: {vehiculoCreateDto.CapacidadBateria}");
            Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");
            
            if (!ModelState.IsValid)
            {
                Console.WriteLine("=== MODEL STATE ERRORS ===");
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"{error.Key}: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                return BadRequest(ModelState);
            }

            try
            {
                var vehiculo = await _vehiculoService.CreateAsync(vehiculoCreateDto);
                return CreatedAtAction(nameof(GetById), new { id = vehiculo.Id }, vehiculo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"=== EXCEPTION ===");
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return BadRequest($"Error al crear el vehículo: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<VehiculoResponseDto>> Update(int id, VehiculoCreateDto vehiculoUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vehiculo = await _vehiculoService.UpdateAsync(id, vehiculoUpdateDto);
            if (vehiculo == null)
                return NotFound($"Vehículo con ID {id} no encontrado");

            return Ok(vehiculo);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _vehiculoService.DeleteAsync(id);
            if (!deleted)
                return NotFound($"Vehículo con ID {id} no encontrado");

            return NoContent();
        }
    }
}
