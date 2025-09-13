using Taller_TIC1_Backend.Models;

namespace Taller_TIC1_Backend.Repositories.Interfaces
{
    public interface IVehiculoRepository
    {
        Task<IEnumerable<Vehiculo>> GetAllAsync();
        Task<Vehiculo?> GetByIdAsync(int id);
        Task<Vehiculo> CreateAsync(Vehiculo vehiculo);
        Task<Vehiculo?> UpdateAsync(int id, Vehiculo vehiculo);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<Vehiculo?> GetByPlacaAsync(string placa);
    }
}
