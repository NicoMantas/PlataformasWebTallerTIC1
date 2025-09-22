using Taller_TIC1_Backend.Models.DTOs;

namespace Taller_TIC1_Backend.Services.Interfaces
{
    public interface IOrdenTrabajoService
    {
        Task<IEnumerable<OrdenTrabajoDTO>> GetAllOrdenesAsync();
        Task<OrdenTrabajoDTO?> GetOrdenByIdAsync(int id);
        Task<OrdenTrabajoDTO> CreateOrdenAsync(OrdenTrabajoCreateDTO ordenDto);
        Task<OrdenTrabajoDTO> UpdateOrdenAsync(int id, OrdenTrabajoDTO ordenDto);
        Task<bool> DeleteOrdenAsync(int id);
        Task<bool> AddServicioToOrdenAsync(int ordenId, int servicioId);
        Task<bool> RemoveServicioFromOrdenAsync(int ordenId, int servicioId);
    }
}
