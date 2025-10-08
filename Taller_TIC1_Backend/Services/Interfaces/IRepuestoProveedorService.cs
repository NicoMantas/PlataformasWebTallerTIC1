using Taller_TIC1_Backend.Models.DTOs;

namespace Taller_TIC1_Backend.Services.Interfaces
{
    public interface IRepuestoProveedorService
    {
        Task<IEnumerable<RepuestoProveedorResponseDto>> GetAllAsync();
        Task<RepuestoProveedorResponseDto?> GetByIdAsync(int idRepuesto, int idProveedor);
        Task<RepuestoProveedorResponseDto> CreateAsync(RepuestoProveedorCreateDto dto);
        Task<bool> DeleteAsync(int idRepuesto, int idProveedor);
        Task<IEnumerable<RepuestoProveedorResponseDto>> GetByRepuestoIdAsync(int idRepuesto);
        Task<IEnumerable<RepuestoProveedorResponseDto>> GetByProveedorIdAsync(int idProveedor);
    }
}
