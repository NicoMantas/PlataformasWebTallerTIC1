using Taller_TIC1_Backend.Models.DTOs;

namespace Taller_TIC1_Backend.Services.Interfaces
{
    public interface ITallerService
    {
        Task<IEnumerable<TallerResponseDto>> GetAllAsync();
        Task<TallerResponseDto?> GetByIdAsync(int id);
        Task<TallerResponseDto> CreateAsync(TallerCreateDto tallerCreateDto);
        Task<bool> ExistsAsync(int id);
    }
}
