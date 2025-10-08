using Taller_TIC1_Backend.Models.DTOs;

namespace Taller_TIC1_Backend.Services.Interfaces
{
    public interface IEmpleadoService
    {
        Task<IEnumerable<EmpleadoResponseDto>> GetAllAsync();
        Task<EmpleadoResponseDto?> GetByIdAsync(int id);
        Task<EmpleadoResponseDto> CreateAsync(EmpleadoCreateDto empleadoCreateDto);
        Task<EmpleadoResponseDto?> UpdateAsync(int id, EmpleadoUpdateDto empleadoUpdateDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<EmpleadoResponseDto?> GetByCedulaAsync(long cedula);
    }
}
