using Microsoft.EntityFrameworkCore;
using Taller_TIC1_Backend.Data;
using Taller_TIC1_Backend.Models;
using Taller_TIC1_Backend.Repositories.Interfaces;

namespace Taller_TIC1_Backend.Repositories
{
    public class RepuestoProveedorRepository : IRepuestoProveedorRepository
    {
        private readonly ApplicationDbContext _context;

        public RepuestoProveedorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RepuestoProveedor>> GetAllAsync()
        {
            return await _context.RepuestoProveedores
                .Include(rp => rp.Repuesto)
                .Include(rp => rp.Proveedor)
                .ToListAsync();
        }

        public async Task<RepuestoProveedor?> GetByIdAsync(int idRepuesto, int idProveedor)
        {
            return await _context.RepuestoProveedores
                .Include(rp => rp.Repuesto)
                .Include(rp => rp.Proveedor)
                .FirstOrDefaultAsync(rp => rp.IdRepuesto == idRepuesto && rp.IdProveedor == idProveedor);
        }

        public async Task<RepuestoProveedor> CreateAsync(RepuestoProveedor repuestoProveedor)
        {
            _context.RepuestoProveedores.Add(repuestoProveedor);
            await _context.SaveChangesAsync();
            return repuestoProveedor;
        }

        public async Task<bool> DeleteAsync(int idRepuesto, int idProveedor)
        {
            var entity = await _context.RepuestoProveedores
                .FirstOrDefaultAsync(rp => rp.IdRepuesto == idRepuesto && rp.IdProveedor == idProveedor);

            if (entity == null) return false;

            _context.RepuestoProveedores.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int idRepuesto, int idProveedor)
        {
            return await _context.RepuestoProveedores
                .AnyAsync(rp => rp.IdRepuesto == idRepuesto && rp.IdProveedor == idProveedor);
        }

        public async Task<IEnumerable<RepuestoProveedor>> GetByRepuestoIdAsync(int idRepuesto)
        {
            return await _context.RepuestoProveedores
                .Include(rp => rp.Proveedor)
                .Where(rp => rp.IdRepuesto == idRepuesto)
                .ToListAsync();
        }

        public async Task<IEnumerable<RepuestoProveedor>> GetByProveedorIdAsync(int idProveedor)
        {
            return await _context.RepuestoProveedores
                .Include(rp => rp.Repuesto)
                .Where(rp => rp.IdProveedor == idProveedor)
                .ToListAsync();
        }
    }
}
