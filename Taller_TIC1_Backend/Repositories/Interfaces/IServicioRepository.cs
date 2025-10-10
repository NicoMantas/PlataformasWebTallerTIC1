using Taller_TIC1_Backend.Models;

namespace Taller_TIC1_Backend.Repositories.Interfaces
{
    public interface IServicioRepository
    {
        Task<IEnumerable<Servicio>> GetAllAsync();
        Task<Servicio?> GetByIdAsync(int id);
        Task<int> GetNextIdAsync();
        Task<Servicio> CreateAsync(Servicio servicio);
        Task<Servicio> UpdateAsync(Servicio servicio);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Servicio>> GetByClienteAsync(int clienteId);
        Task<IEnumerable<Servicio>> GetByClienteAndEstadosAsync(int clienteId, IEnumerable<int> estadosIds);
        Task<int?> GetEstadoIdByDescripcionAsync(string descripcion);
        Task<int> EnsureEstadoAsync(string descripcion);
        Task<IEnumerable<Servicio>> GetActivosPendientesAsync(IEnumerable<int> estadosActivosIds);
        Task<IEnumerable<Servicio>> GetActivosAsignadosAsync(IEnumerable<int> estadosActivosIds);
        Task<bool> AssignEmpleadoAsync(int servicioId, int empleadoId);
        Task<DetalleRevision?> GetDetalleRevisionByServicioIdAsync(int servicioId);
        Task<DetalleReparacion?> GetDetalleReparacionByServicioIdAsync(int servicioId);
        Task<IEnumerable<DetalleReparacionRepuesto>> GetRepuestosByDetalleReparacionAsync(int idDetalleReparacionRepuesto);
        Task<DetalleRevision> CreateDetalleRevisionAsync(DetalleRevision detalleRevision);
        Task<DetalleReparacion> CreateDetalleReparacionAsync(DetalleReparacion detalleReparacion);
        Task<DetalleReparacionRepuesto> CreateDetalleReparacionRepuestoAsync(DetalleReparacionRepuesto detalleReparacionRepuesto);
        Task<IEnumerable<Servicio>> GetServiciosByEmpleadoAndEstadosAsync(int empleadoId, IEnumerable<int> estadosIds);
        Task<bool> UpdateServicioEstadoAsync(int servicioId, int estadoId);
        Task<bool> DesasignarEmpleadoAsync(int servicioId);
        Task<IEnumerable<Servicio>> GetByVehiculoAsync(int vehiculoId);
        
        // Métodos de debug
        Task<IEnumerable<EstadoServicio>> GetAllEstadosAsync();
        Task<IEnumerable<Servicio>> GetAllServiciosByEmpleadoAsync(int empleadoId);
    }
}
