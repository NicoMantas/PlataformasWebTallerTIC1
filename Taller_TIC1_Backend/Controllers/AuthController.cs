using Microsoft.AspNetCore.Mvc;
using Taller_TIC1_Backend.Models;
using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Services.Interfaces;

namespace Taller_TIC1_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller 
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // Autenticación
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginRequestDto loginRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(loginRequest);

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }

        [HttpPost("register/empleado")]
        public async Task<ActionResult<AuthResponseDto>> RegisterEmpleado(RegisterRequestDto registerRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterEmpleadoAsync(registerRequest);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("register/cliente")]
        public async Task<ActionResult<AuthResponseDto>> RegisterCliente(RegisterRequestDto registerRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterClienteAsync(registerRequest);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // CRUD para UsuarioEmpleadoTaller
        [HttpGet("empleados")]
        public async Task<ActionResult<IEnumerable<UsuarioEmpleadoTaller>>> GetUsuariosEmpleado()
        {
            var usuarios = await _authService.GetAllUsuariosEmpleadoAsync();
            return Ok(usuarios);
        }

        [HttpGet("empleados/{id}")]
        public async Task<ActionResult<UsuarioEmpleadoTaller>> GetUsuarioEmpleado(int id)
        {
            var usuario = await _authService.GetUsuarioEmpleadoByIdAsync(id);
            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        [HttpPut("empleados/{id}")]
        public async Task<ActionResult<AuthResponseDto>> UpdateUsuarioEmpleado(int id, UpdateUsuarioEmpleadoDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.UpdateUsuarioEmpleadoAsync(id, updateDto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("empleados/{id}")]
        public async Task<ActionResult> DeleteUsuarioEmpleado(int id)
        {
            var result = await _authService.DeleteUsuarioEmpleadoAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        // CRUD para UsuarioClienteTaller
        [HttpGet("clientes")]
        public async Task<ActionResult<IEnumerable<UsuarioClienteTaller>>> GetUsuariosCliente()
        {
            var usuarios = await _authService.GetAllUsuariosClienteAsync();
            return Ok(usuarios);
        }

        [HttpGet("clientes/{id}")]
        public async Task<ActionResult<UsuarioClienteTaller>> GetUsuarioCliente(int id)
        {
            var usuario = await _authService.GetUsuarioClienteByIdAsync(id);
            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        [HttpPut("clientes/{id}")]
        public async Task<ActionResult<AuthResponseDto>> UpdateUsuarioCliente(int id, UpdateUsuarioClienteDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.UpdateUsuarioClienteAsync(id, updateDto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("clientes/{id}")]
        public async Task<ActionResult> DeleteUsuarioCliente(int id)
        {
            var result = await _authService.DeleteUsuarioClienteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("email-exists/{email}")]
        public async Task<ActionResult<bool>> EmailExists(string email)
        {
            var exists = await _authService.EmailExistsAsync(email);
            return Ok(exists);
        }

    }

}

