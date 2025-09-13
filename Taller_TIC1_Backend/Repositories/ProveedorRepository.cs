using Microsoft.EntityFrameworkCore;
using B_TallerAutomoviles.Clases;
using Taller_TIC1_Backend.Data;
using Taller_TIC1_Backend.Repositories.Interfaces;

namespace Taller_TIC1_Backend.Repositories
{
    public class ProveedorRepository : IProveedorRepository

    {
        private readonly ApplicationDbContext _context;

        public ProveedorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Proveedor>> GetAllAsync()
        {
            return await _context.Proveedores
                .ToListAsync();
        } 

        public async Task<Proveedor?> GetByIdAsync(int id)
        {
            return await _context.Proveedores
                .FirstOrDefaultAsync(p => p.Id == id);
        } 

        public async Task<Proveedor> CreateAsync(Proveedor proveedor)
        {
            _context.Proveedores.Add(proveedor);
            await _context.SaveChangesAsync();
            return proveedor;
        }

        public async Task<Proveedor?> UpdateAsync(Proveedor proveedor)
        {
            var existingProveedor = await _context.Proveedores.FindAsync(proveedor.Id);
            if (existingProveedor == null)
                return null;

            _context.Entry(existingProveedor).CurrentValues.SetValues(proveedor);
            await _context.SaveChangesAsync();
            return existingProveedor;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null)
                return false;

            _context.Proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Proveedores.AnyAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Proveedor>> SearchByNameAsync(string name)
        {
            return await _context.Proveedores
                .Where(p => p.Nombre.Contains(name))
                .ToListAsync();
        } 
    }
}
