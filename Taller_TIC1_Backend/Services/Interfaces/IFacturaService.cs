using Taller_TIC1_Backend.Models.DTOs;

namespace Taller_TIC1_Backend.Services.Interfaces
{
    public interface IFacturaService
    {
        Task<IEnumerable<FacturaDTO>> GetAllFacturasAsync();
        Task<FacturaDTO?> GetFacturaByIdAsync(int id);
        Task<FacturaDTO> CreateFacturaAsync(FacturaCreateDTO facturaDto);
        Task<FacturaDTO> UpdateFacturaAsync(int id, FacturaDTO facturaDto);
        Task<bool> DeleteFacturaAsync(int id);
        Task<FacturaDTO?> GetFacturaByOrdenIdAsync(int ordenId);
    }
}
