using Taller_TIC1_Backend.Models.DTOs;

namespace Taller_TIC1_Backend.Services.Interfaces
{
    public interface ITallerService
    {
        Task<IEnumerable<TallerResponseDto>> GetAllAsync();
        Task<TallerResponseDto?> GetByIdAsync(int id);
        Task<TallerResponseDto> CreateAsync(TallerCreateDto tallerCreateDto);
        Task<bool> ExistsAsync(int id);
        Task<TallerResponseDto?> UpdateAsync(int id, TallerUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
