using Microsoft.AspNetCore.Mvc;
using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Services.Interfaces;

namespace Taller_TIC1_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteResponseDto>>> GetAll()
        {
            var clientes = await _clienteService.GetAllAsync();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteResponseDto>> GetById(int id)
        {
            var cliente = await _clienteService.GetByIdAsync(id);
            if (cliente == null)
                return NotFound($"Cliente con ID {id} no encontrado");

            return Ok(cliente);
        }

        [HttpPost]
        public async Task<ActionResult<ClienteResponseDto>> Create(ClienteCreateDto clienteCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Intentar crear el cliente
                var cliente = await _clienteService.CreateAsync(clienteCreateDto); // Crear el cliente
                return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente); // Retornar 201 Created con la ubicaci�n del nuevo recurso
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear el cliente: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ClienteResponseDto>> Update(int id, ClienteUpdateDto clienteUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cliente = await _clienteService.UpdateAsync(id, clienteUpdateDto);
            if (cliente == null)
                return NotFound($"Cliente con ID {id} no encontrado");

            return Ok(cliente);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _clienteService.DeleteAsync(id);
            if (!deleted)
                return NotFound($"Cliente con ID {id} no encontrado");

            return Ok(new { message = "Cliente desactivado exitosamente" });
        }

        [HttpPatch("{id}/activate")]
        public async Task<ActionResult> Activate(int id)
        {
            var activated = await _clienteService.ActivateAsync(id);
            if (!activated)
                return NotFound($"Cliente con ID {id} no encontrado");

            return Ok(new { message = "Cliente activado exitosamente" });
        }
    }
}
