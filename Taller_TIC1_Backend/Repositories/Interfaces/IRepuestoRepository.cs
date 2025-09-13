using Taller_TIC1_Backend.Models;

namespace Taller_TIC1_Backend.Repositories.Interfaces
{
    public interface IRepuestoRepository
    {
        Task<IEnumerable<Repuesto>> GetAllAsync();
        Task<Repuesto?> GetByIdAsync(int id);
        Task<Repuesto> CreateAsync(Repuesto repuesto);
        Task<Repuesto?> UpdateAsync(Repuesto repuesto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Repuesto>> SearchByNameAsync(string name);
        Task<IEnumerable<Repuesto>> GetByStockAsync(int minStock);
        Task<Repuesto?> GetByNumeroSerieAsync(long numeroSerie);
    }
}

