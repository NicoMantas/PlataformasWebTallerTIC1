using Taller_TIC1_Backend.Models.DTOs;
using static Taller_TIC1_Backend.Models.DTOs.PorveedorCreateDTOcs;
namespace Taller_TIC1_Backend.Services.Interfaces
{
    public interface IProveedorService
    {
        Task<IEnumerable<ProveedorResponseDto>> GetAllAsync();
        Task<ProveedorResponseDto?> GetByIdAsync(int id);
        Task<ProveedorResponseDto> CreateAsync(ProveedorCreateDto dto);
        Task<ProveedorResponseDto?> UpdateAsync(ProveedorUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<ProveedorResponseDto>> SearchByNameAsync(string name);
    }
}
