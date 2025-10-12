using Microsoft.AspNetCore.Mvc;
using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Services.Interfaces;

namespace Taller_TIC1_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacturaController : ControllerBase
    {
        private readonly IFacturaService _facturaService;

        public FacturaController(IFacturaService facturaService)
        {
            _facturaService = facturaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FacturaDTO>>> GetFacturas()
        {
            var facturas = await _facturaService.GetAllFacturasAsync();
            return Ok(facturas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FacturaDTO>> GetFactura(int id)
        {
            var factura = await _facturaService.GetFacturaByIdAsync(id);
            if (factura == null) return NotFound();
            return Ok(factura);
        }

        [HttpPost]
        public async Task<ActionResult<FacturaDTO>> CreateFactura(FacturaCreateDTO facturaDto)
        {
            var factura = await _facturaService.CreateFacturaAsync(facturaDto);
            return CreatedAtAction(nameof(GetFactura), new { id = factura.Id }, factura);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFactura(int id, FacturaDTO facturaDto)
        {
            if (id != facturaDto.Id) return BadRequest();

            try
            {
                var factura = await _facturaService.UpdateFacturaAsync(id, facturaDto);
                return Ok(factura);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFactura(int id)
        {
            var result = await _facturaService.DeleteFacturaAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("orden/{ordenId}")]
        public async Task<ActionResult<FacturaDTO>> GetFacturaByOrden(int ordenId)
        {
            var factura = await _facturaService.GetFacturaByOrdenIdAsync(ordenId);
            if (factura == null) return NotFound();
            return Ok(factura);
        }

        [HttpGet("servicio/{servicioId}")]
        public async Task<ActionResult<FacturaDTO>> GetFacturaByServicio(int servicioId)
        {
            var factura = await _facturaService.GetFacturaByServicioIdAsync(servicioId);
            if (factura == null) return NotFound();
            return Ok(factura);
        }
    }
}
