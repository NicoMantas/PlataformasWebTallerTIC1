using Taller_TIC1_Backend.Models;

namespace Taller_TIC1_Backend.Repositories.Interfaces
{
    public interface IServicioRepository
    {
        Task<IEnumerable<Servicio>> GetAllAsync();
        Task<Servicio?> GetByIdAsync(int id);
        Task<Servicio> CreateAsync(Servicio servicio);
        Task<Servicio> UpdateAsync(Servicio servicio);
        Task<bool> DeleteAsync(int id);
        Task<DetalleRevision?> GetDetalleRevisionByServicioIdAsync(int servicioId);
        Task<DetalleReparacion?> GetDetalleReparacionByServicioIdAsync(int servicioId);
        Task<IEnumerable<DetalleReparacionRepuesto>> GetRepuestosByDetalleReparacionAsync(int idDetalleReparacionRepuesto);
        Task<DetalleRevision> CreateDetalleRevisionAsync(DetalleRevision detalleRevision);
        Task<DetalleReparacion> CreateDetalleReparacionAsync(DetalleReparacion detalleReparacion);
        Task<DetalleReparacionRepuesto> CreateDetalleReparacionRepuestoAsync(DetalleReparacionRepuesto detalleReparacionRepuesto);
    }
}
