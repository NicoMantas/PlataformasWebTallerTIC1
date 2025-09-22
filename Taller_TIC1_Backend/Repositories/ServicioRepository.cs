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
                .Include(drr => drr.IdRepuesto)
                .ToListAsync();
        }
    }

    public class OrdenTrabajoRepository : IOrdenTrabajoRepository
    {
        private readonly ApplicationDbContext _context;

        public OrdenTrabajoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrdenDeTrabajo>> GetAllAsync()
        {
            return await _context.OrdenesDeTrabajo
                .Include(o => o.TipoEstadoOrden)
                .ToListAsync();
        }

        public async Task<OrdenDeTrabajo?> GetByIdAsync(int id)
        {
            return await _context.OrdenesDeTrabajo
                .Include(o => o.TipoEstadoOrden)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<OrdenDeTrabajo> CreateAsync(OrdenDeTrabajo orden)
        {
            _context.OrdenesDeTrabajo.Add(orden);
            await _context.SaveChangesAsync();
            return orden;
        }

        public async Task<OrdenDeTrabajo> UpdateAsync(OrdenDeTrabajo orden)
        {
            _context.OrdenesDeTrabajo.Update(orden);
            await _context.SaveChangesAsync();
            return orden;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var orden = await _context.OrdenesDeTrabajo.FindAsync(id);
            if (orden == null) return false;

            _context.OrdenesDeTrabajo.Remove(orden);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Servicio>> GetServiciosByOrdenIdAsync(int ordenId)
        {
            var detalles = await _context.DetallesServicioOrden
                .Where(dso => dso.IdOrdenTrabajo == ordenId)
                .Include(dso => dso.Servicio)
                .ThenInclude(s => s.Cliente)
                .Include(dso => dso.Servicio)
                .ThenInclude(s => s.Empleado)
                .Include(dso => dso.Servicio)
                .ThenInclude(s => s.Estado)
                .ToListAsync();

            return detalles.Select(d => d.Servicio);
        }

        public async Task<bool> AddServicioToOrdenAsync(int ordenId, int servicioId)
        {
            var existe = await _context.DetallesServicioOrden
                .AnyAsync(dso => dso.IdOrdenTrabajo == ordenId && dso.IdServicio == servicioId);

            if (!existe)
            {
                var detalle = new DetalleServicioOrden
                {
                    IdOrdenTrabajo = ordenId,
                    IdServicio = servicioId
                };
                _context.DetallesServicioOrden.Add(detalle);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveServicioFromOrdenAsync(int ordenId, int servicioId)
        {
            var detalle = await _context.DetallesServicioOrden
                .FirstOrDefaultAsync(dso => dso.IdOrdenTrabajo == ordenId && dso.IdServicio == servicioId);

            if (detalle != null)
            {
                _context.DetallesServicioOrden.Remove(detalle);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }

}

