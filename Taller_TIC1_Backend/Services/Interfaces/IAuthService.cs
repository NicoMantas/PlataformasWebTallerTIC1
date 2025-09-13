using Taller_TIC1_Backend.Models.DTOs;

namespace Taller_TIC1_Backend.Services.Interfaces
{
    public interface IAuthService // Interfaz para el servicio de autenticación
    {
        Task<AuthResponseDto> LoginAsync(LoginRequestDto loginRequest);
        Task<AuthResponseDto> RegisterEmpleadoAsync(RegisterRequestDto registerRequest);
        Task<AuthResponseDto> RegisterClienteAsync(RegisterRequestDto registerRequest);
        Task<bool> EmailExistsAsync(string email);
    }
}
