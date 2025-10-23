using Taller_TIC1_Backend.Models.DTOs;

namespace Taller_TIC1_Backend.Services.Interfaces
{
    public interface IEstadisticasAdminService
    {
        Task<EstadisticasAdminDto> GetEstadisticasCompletasAsync();
        Task<ResumenGeneralDto> GetResumenGeneralAsync();
        Task<EstadisticasServiciosDto> GetEstadisticasServiciosAsync();
        Task<EstadisticasEmpleadosDto> GetEstadisticasEmpleadosAsync();
        Task<EstadisticasFacturacionDto> GetEstadisticasFacturacionAsync();
        Task<List<ReporteMensualDto>> GetReportesMensualesAsync(int mesesAtras = 12);
        Task<object> GetCapacidadTallerDetalladaAsync();
        Task<object> GetDebugServiciosCompletadosAsync();
        Task<object> GetDebugTodosServiciosAsync();
        Task<object> GetDebugEstadosServiciosAsync();
        Task<object> GetDebugCapacidadTallerAsync();
    }
}
