using Microsoft.AspNetCore.Mvc;
using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Services.Interfaces;

namespace Taller_TIC1_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller 
    {
        // Controlador para manejar las rutas de autenticación
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        //endpoint para login
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

        [HttpGet("email-exists/{email}")]
        public async Task<ActionResult<bool>> EmailExists(string email)
        {
            var exists = await _authService.EmailExistsAsync(email);
            return Ok(exists);
        }
    }

}

