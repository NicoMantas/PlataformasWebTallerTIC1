using Microsoft.EntityFrameworkCore;
using B_TallerAutomoviles.Clases;
using Taller_TIC1_Backend.Data;
using Taller_TIC1_Backend.Repositories.Interfaces;

namespace Taller_TIC1_Backend.Repositories
{
    public class RepuestoRepository : IRepuestoRepository
    {
        private readonly ApplicationDbContext _context;

        public RepuestoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Repuesto>> GetAllAsync()
        {
            return await _context.Repuestos
                .Include(r => r.Proveedores)
                .ToListAsync();
        }

        public async Task<Repuesto?> GetByIdAsync(int id)
        {
            return await _context.Repuestos
                .Include(r => r.Proveedores)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Repuesto> CreateAsync(Repuesto repuesto)
        {
            _context.Repuestos.Add(repuesto);
            await _context.SaveChangesAsync();
            return repuesto;
        }

        public async Task<Repuesto?> UpdateAsync(Repuesto repuesto)
        {
            var existing = await _context.Repuestos.FindAsync(repuesto.Id);
            if (existing == null) return null;
            _context.Entry(existing).CurrentValues.SetValues(repuesto);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Repuestos.FindAsync(id);
            if (entity == null) return false;
            _context.Repuestos.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Repuestos.AnyAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Repuesto>> SearchByNameAsync(string name)
        {
            return await _context.Repuestos
                .Include(r => r.Proveedores)
                .Where(r => r.Nombre.Contains(name))
                .ToListAsync();
        }

        public async Task<IEnumerable<Repuesto>> GetByStockAsync(int minStock)
        {
            return await _context.Repuestos
                .Include(r => r.Proveedores)
                .Where(r => r.Stock >= minStock)
                .ToListAsync();
        }

        public async Task<Repuesto?> GetByNumeroSerieAsync(long numeroSerie)
        {
            return await _context.Repuestos
                .Include(r => r.Proveedores)
                .FirstOrDefaultAsync(r => r.Numero_serie == numeroSerie);
        }
    }
}

