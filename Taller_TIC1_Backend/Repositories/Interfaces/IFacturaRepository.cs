using Taller_TIC1_Backend.Models;

namespace Taller_TIC1_Backend.Repositories.Interfaces
{
    public interface IFacturaRepository
    {
        Task<IEnumerable<Factura>> GetAllAsync();
        Task<Factura?> GetByIdAsync(int id);
        Task<Factura> CreateAsync(Factura factura);
        Task<Factura> UpdateAsync(Factura factura);
        Task<bool> DeleteAsync(int id);
        Task<Factura?> GetByOrdenIdAsync(int ordenId);
    }
}
