using Taller_TIC1_Backend.Models;

namespace Taller_TIC1_Backend.Repositories.Interfaces

{
    public interface IOrdenTrabajoRepository
    {
        Task<IEnumerable<OrdenDeTrabajo>> GetAllAsync();
        Task<OrdenDeTrabajo?> GetByIdAsync(int id);
        Task<OrdenDeTrabajo> CreateAsync(OrdenDeTrabajo orden);
        Task<OrdenDeTrabajo> UpdateAsync(OrdenDeTrabajo orden);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Servicio>> GetServiciosByOrdenIdAsync(int ordenId);
        Task<bool> AddServicioToOrdenAsync(int ordenId, int servicioId);
        Task<bool> RemoveServicioFromOrdenAsync(int ordenId, int servicioId);
    }
}
