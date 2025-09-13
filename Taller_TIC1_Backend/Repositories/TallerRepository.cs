using Microsoft.EntityFrameworkCore;
using Taller_TIC1_Backend.Data;
using Taller_TIC1_Backend.Models;
using Taller_TIC1_Backend.Repositories.Interfaces;


namespace Taller_TIC1_Backend.Repositories
{
    public class TallerRepository : ITallerRepository
    {
        private readonly ApplicationDbContext _context;

        public TallerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Taller>> GetAllAsync()
        {
            return await _context.Talleres.ToListAsync();
        }

        public async Task<Taller?> GetByIdAsync(int id)
        {
            return await _context.Talleres.FindAsync(id);
        }

        public async Task<Taller> CreateAsync(Taller taller)
        {
            _context.Talleres.Add(taller);
            await _context.SaveChangesAsync();
            return taller;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Talleres.AnyAsync(t => t.Id == id);
        }
    }
}
