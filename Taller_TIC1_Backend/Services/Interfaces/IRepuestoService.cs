using Taller_TIC1_Backend.Models.DTOs;

namespace Taller_TIC1_Backend.Services.Interfaces
{
    public interface IRepuestoService
    {
        Task<IEnumerable<RepuestoResponseDto>> GetAllAsync();
        Task<RepuestoResponseDto?> GetByIdAsync(int id);
        Task<RepuestoResponseDto> CreateAsync(RepuestoCreateDto dto);
        Task<RepuestoResponseDto?> UpdateAsync(RepuestoUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<RepuestoResponseDto>> SearchByNameAsync(string name);
        Task<IEnumerable<RepuestoResponseDto>> GetByStockAsync(int minStock);
        Task<RepuestoResponseDto?> GetByNumeroSerieAsync(long numeroSerie);
    }
}

