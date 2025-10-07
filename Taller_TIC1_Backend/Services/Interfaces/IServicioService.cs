using Taller_TIC1_Backend.Models.DTOs;

namespace Taller_TIC1_Backend.Services.Interfaces
{
    public interface IServicioService
    {
        Task<IEnumerable<ServicioDTO>> GetAllServiciosAsync();
        Task<ServicioDTO?> GetServicioByIdAsync(int id);
        Task<ServicioDTO> CreateServicioAsync(ServicioCreateDTO servicioDto);
        Task<ServicioDTO> UpdateServicioAsync(int id, ServicioDTO servicioDto);
        Task<bool> DeleteServicioAsync(int id);
        Task<IEnumerable<ServicioDTO>> GetServiciosByClienteAsync(int clienteId);
        Task<IEnumerable<ServicioDTO>> GetServiciosActivosByClienteAsync(int clienteId);
        Task<IEnumerable<ServicioDTO>> GetServiciosHistorialByClienteAsync(int clienteId);
        Task<bool> CancelarServicioAsync(int id);
        Task<IEnumerable<ServicioDTO>> SecretariaListPendientesAsync();
        Task<IEnumerable<ServicioDTO>> SecretariaListAsignadosAsync();
        Task<bool> SecretariaAsignarMecanicoAsync(int servicioId, int empleadoId);
    }
}
