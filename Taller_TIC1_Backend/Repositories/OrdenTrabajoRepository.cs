using Microsoft.EntityFrameworkCore;
using Taller_TIC1_Backend.Data;
using Taller_TIC1_Backend.Models;
using Taller_TIC1_Backend.Repositories.Interfaces;

namespace Taller_TIC1_Backend.Repositories
{
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
