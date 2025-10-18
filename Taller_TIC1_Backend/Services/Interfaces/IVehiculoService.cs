using Taller_TIC1_Backend.Models.DTOs;

namespace Taller_TIC1_Backend.Services.Interfaces
{
    public interface IVehiculoService
    {
        Task<IEnumerable<VehiculoResponseDto>> GetAllAsync();
        Task<VehiculoResponseDto?> GetByIdAsync(int id);
        Task<VehiculoResponseDto> CreateAsync(VehiculoCreateDto vehiculoCreateDto);
        Task<VehiculoResponseDto?> UpdateAsync(int id, VehiculoUpdateDto vehiculoUpdateDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<VehiculoResponseDto?> GetByPlacaAsync(string placa);
    }
}
