using Taller_TIC1_Backend.Models;

namespace Taller_TIC1_Backend.Repositories.Interfaces
{
    public interface IRepuestoProveedorRepository
    {
        Task<IEnumerable<RepuestoProveedor>> GetAllAsync();
        Task<RepuestoProveedor?> GetByIdAsync(int idRepuesto, int idProveedor);
        Task<RepuestoProveedor> CreateAsync(RepuestoProveedor repuestoProveedor);
        Task<bool> DeleteAsync(int idRepuesto, int idProveedor);
        Task<bool> ExistsAsync(int idRepuesto, int idProveedor);
        Task<IEnumerable<RepuestoProveedor>> GetByRepuestoIdAsync(int idRepuesto);
        Task<IEnumerable<RepuestoProveedor>> GetByProveedorIdAsync(int idProveedor);
    }
}
