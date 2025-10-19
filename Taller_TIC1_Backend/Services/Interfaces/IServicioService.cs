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
        Task<IEnumerable<ServicioDTO>> SecretariaListCompletadosAsync();
        Task<bool> SecretariaAsignarMecanicoAsync(int servicioId, int empleadoId);
        Task<IEnumerable<ServicioDTO>> GetServiciosByMecanicoAsync(int empleadoId);
        Task<IEnumerable<ServicioDTO>> GetServiciosCompletadosByMecanicoAsync(int empleadoId);
        Task<bool> UpdateServicioEstadoAsync(int servicioId, int estadoId);
        Task<bool> DesasignarEmpleadoAsync(int servicioId);
        Task<IEnumerable<ServicioDTO>> GetHistorialByVehiculoAsync(int vehiculoId);
        Task<decimal> CalcularCostoTotalReparacionAsync(int servicioId, decimal costoBase);
        Task<object> GetCapacidadTallerAsync();
        Task<IEnumerable<ServicioDTO>> GetServiciosByPlacaAsync(string placa);
        Task<IEnumerable<ServicioDTO>> GetServiciosByFechaAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<IEnumerable<ServicioDTO>> GetServiciosByPlacaAndFechaAsync(string placa, DateTime fechaInicio, DateTime fechaFin);
        Task<ServicioProgresoDto?> GetProgresoServicioAsync(int servicioId);
        
    }
}
