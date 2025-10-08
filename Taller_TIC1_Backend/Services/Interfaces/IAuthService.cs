using Taller_TIC1_Backend.Models;
using Taller_TIC1_Backend.Models.DTOs;

namespace Taller_TIC1_Backend.Services.Interfaces
{
    public interface IAuthService // Interfaz para el servicio de autenticación
    {
        Task<AuthResponseDto> LoginAsync(LoginRequestDto loginRequest);
        Task<AuthResponseDto> RegisterEmpleadoAsync(RegisterRequestDto registerRequest);
        Task<AuthResponseDto> RegisterClienteAsync(RegisterRequestDto registerRequest);
        Task<bool> EmailExistsAsync(string email);

        // CRUD para UsuarioEmpleadoTaller
        Task<IEnumerable<UsuarioEmpleadoTaller>> GetAllUsuariosEmpleadoAsync();
        Task<UsuarioEmpleadoTaller?> GetUsuarioEmpleadoByIdAsync(int id);
        Task<AuthResponseDto> UpdateUsuarioEmpleadoAsync(int id, UpdateUsuarioEmpleadoDto updateDto);
        Task<bool> DeleteUsuarioEmpleadoAsync(int id);

        // CRUD para UsuarioClienteTaller
        Task<IEnumerable<UsuarioClienteTaller>> GetAllUsuariosClienteAsync();
        Task<UsuarioClienteTaller?> GetUsuarioClienteByIdAsync(int id);
        Task<AuthResponseDto> UpdateUsuarioClienteAsync(int id, UpdateUsuarioClienteDto updateDto);
        Task<bool> DeleteUsuarioClienteAsync(int id);
    }
}
