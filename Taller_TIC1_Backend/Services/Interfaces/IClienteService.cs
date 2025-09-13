using Taller_TIC1_Backend.Models;
using Taller_TIC1_Backend.Models.DTOs;

namespace Taller_TIC1_Backend.Services.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<ClienteResponseDto>> GetAllAsync();
        Task<ClienteResponseDto?> GetByIdAsync(int id);
        Task<ClienteResponseDto> CreateAsync(ClienteCreateDto clienteCreateDto);
        Task<ClienteResponseDto?> UpdateAsync(int id, ClienteUpdateDto clienteUpdateDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
