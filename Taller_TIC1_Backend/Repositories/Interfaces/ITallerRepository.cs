using Taller_TIC1_Backend.Models;

namespace Taller_TIC1_Backend.Repositories.Interfaces
{
    public interface ITallerRepository
    {
        Task<IEnumerable<Taller>> GetAllAsync();
        Task<Taller?> GetByIdAsync(int id);
        Task<Taller> CreateAsync(Taller taller);
        Task<bool> ExistsAsync(int id);
    }
}
