using Microsoft.EntityFrameworkCore;
using Taller_TIC1_Backend.Data;
using Taller_TIC1_Backend.Models;
using Taller_TIC1_Backend.Repositories.Interfaces;

namespace Taller_TIC1_Backend.Repositories
{
    public class ServicioRepository : IServicioRepository
    {
        private readonly ApplicationDbContext _context;

        public ServicioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Servicio>> GetAllAsync()
        {
            return await _context.Servicios
                .Include(s => s.Cliente)
                .Include(s => s.Empleado)
                .Include(s => s.Estado)
                .ToListAsync();
        }

        public async Task<Servicio?> GetByIdAsync(int id)
        {
            return await _context.Servicios
                .Include(s => s.Cliente)
                .Include(s => s.Empleado)
                .Include(s => s.Estado)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Servicio> CreateAsync(Servicio servicio)
        {
            _context.Servicios.Add(servicio);
            await _context.SaveChangesAsync();
            return servicio;
        }

        public async Task<Servicio> UpdateAsync(Servicio servicio)
        {
            _context.Servicios.Update(servicio);
            await _context.SaveChangesAsync();
            return servicio;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null) return false;

            _context.Servicios.Remove(servicio);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Servicio>> GetByClienteAsync(int clienteId)
        {
            return await _context.Servicios
                .Include(s => s.Cliente)
                .Include(s => s.Empleado)
                .Include(s => s.Estado)
                .Where(s => s.IdCliente == clienteId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Servicio>> GetByClienteAndEstadosAsync(int clienteId, IEnumerable<int> estadosIds)
        {
            var estados = estadosIds.ToList();
            return await _context.Servicios
                .Include(s => s.Cliente)
                .Include(s => s.Empleado)
                .Include(s => s.Estado)
                .Where(s => s.IdCliente == clienteId && estados.Contains(s.IdEstado))
                .ToListAsync();
        }

        public async Task<int?> GetEstadoIdByDescripcionAsync(string descripcion)
        {
            var estado = await _context.EstadosServicio.FirstOrDefaultAsync(e => e.Descripcion == descripcion);
            return estado?.Id;
        }

        public async Task<int> EnsureEstadoAsync(string descripcion)
        {
            var existing = await _context.EstadosServicio.FirstOrDefaultAsync(e => e.Descripcion == descripcion);
            if (existing != null) return existing.Id;
            // Algunos entornos pueden no tener identidad creada para esta tabla; generamos Id manualmente
            var maxId = await _context.EstadosServicio.Select(e => (int?)e.Id).MaxAsync() ?? 0;
            var nuevo = new EstadoServicio { Id = maxId + 1, Descripcion = descripcion };
            _context.EstadosServicio.Add(nuevo);
            await _context.SaveChangesAsync();
            return nuevo.Id;
        }

        public async Task<IEnumerable<Servicio>> GetActivosPendientesAsync(IEnumerable<int> estadosActivosIds)
        {
            var estados = estadosActivosIds.ToList();
            return await _context.Servicios
                .Include(s => s.Cliente)
                .Include(s => s.Empleado)
                .Include(s => s.Estado)
                .Where(s => estados.Contains(s.IdEstado) && s.IdEmpleado == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Servicio>> GetActivosAsignadosAsync(IEnumerable<int> estadosActivosIds)
        {
            var estados = estadosActivosIds.ToList();
            return await _context.Servicios
                .Include(s => s.Cliente)
                .Include(s => s.Empleado)
                .Include(s => s.Estado)
                .Where(s => estados.Contains(s.IdEstado) && s.IdEmpleado != null)
                .ToListAsync();
        }

        public async Task<bool> AssignEmpleadoAsync(int servicioId, int empleadoId)
        {
            var servicio = await _context.Servicios.FindAsync(servicioId);
            if (servicio == null) return false;
            servicio.IdEmpleado = empleadoId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<DetalleRevision?> GetDetalleRevisionByServicioIdAsync(int servicioId)
        {
            return await _context.DetallesRevision
                .FirstOrDefaultAsync(dr => dr.IdServicio == servicioId);
        }

        public async Task<DetalleReparacion?> GetDetalleReparacionByServicioIdAsync(int servicioId)
        {
            return await _context.DetallesReparacion
                .FirstOrDefaultAsync(dr => dr.IdServicio == servicioId);
        }

        public async Task<IEnumerable<DetalleReparacionRepuesto>> GetRepuestosByDetalleReparacionAsync(int idDetalleReparacionRepuesto)
        {
            return await _context.DetallesReparacionRepuesto
                .Where(drr => drr.Id == idDetalleReparacionRepuesto)
                .Include(drr => drr.Repuesto)
                .ToListAsync();
        }

        public async Task<DetalleRevision> CreateDetalleRevisionAsync(DetalleRevision detalleRevision)
        {
            _context.DetallesRevision.Add(detalleRevision);
            await _context.SaveChangesAsync();
            return detalleRevision;
        }

        public async Task<DetalleReparacion> CreateDetalleReparacionAsync(DetalleReparacion detalleReparacion)
        {
            _context.DetallesReparacion.Add(detalleReparacion);
            await _context.SaveChangesAsync();
            return detalleReparacion;
        }

        public async Task<DetalleReparacionRepuesto> CreateDetalleReparacionRepuestoAsync(DetalleReparacionRepuesto detalleReparacionRepuesto)
        {
            _context.DetallesReparacionRepuesto.Add(detalleReparacionRepuesto);
            await _context.SaveChangesAsync();
            return detalleReparacionRepuesto;
        }
    }

}

