using B_TallerAutomoviles.Clases;

namespace Taller_TIC1_Backend.Repositories.Interfaces
{
    public interface IProveedorRepository
    {
        Task<IEnumerable<Proveedor>> GetAllAsync();
        Task<Proveedor?> GetByIdAsync(int id);
        Task<Proveedor> CreateAsync(Proveedor proveedor);
        Task<Proveedor?> UpdateAsync(Proveedor proveedor);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Proveedor>> SearchByNameAsync(string name);
    }
}
