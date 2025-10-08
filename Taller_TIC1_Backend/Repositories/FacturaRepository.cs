using Microsoft.EntityFrameworkCore;
using Taller_TIC1_Backend.Data;
using Taller_TIC1_Backend.Models;
using Taller_TIC1_Backend.Repositories.Interfaces;

namespace Taller_TIC1_Backend.Repositories
{
    public class FacturaRepository : IFacturaRepository
    {
        private readonly ApplicationDbContext _context;

        public FacturaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Factura>> GetAllAsync()
        {
            return await _context.Facturas
                .Include(f => f.OrdenTrabajo)
                .ToListAsync();
        }

        public async Task<Factura?> GetByIdAsync(int id)
        {
            return await _context.Facturas
                .Include(f => f.OrdenTrabajo)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Factura> CreateAsync(Factura factura)
        {
            _context.Facturas.Add(factura);
            await _context.SaveChangesAsync();
            return factura;
        }

        public async Task<Factura> UpdateAsync(Factura factura)
        {
            _context.Facturas.Update(factura);
            await _context.SaveChangesAsync();
            return factura;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var factura = await _context.Facturas.FindAsync(id);
            if (factura == null) return false;

            _context.Facturas.Remove(factura);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Factura?> GetByOrdenIdAsync(int ordenId)
        {
            return await _context.Facturas
                .Include(f => f.OrdenTrabajo)
                .FirstOrDefaultAsync(f => f.IdOrdenTrabajo == ordenId);
        }
    }
}
