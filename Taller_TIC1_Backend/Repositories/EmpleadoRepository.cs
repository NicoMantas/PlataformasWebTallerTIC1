using Microsoft.EntityFrameworkCore;
using Taller_TIC1_Backend.Data;
using Taller_TIC1_Backend.Models;
using Taller_TIC1_Backend.Repositories.Interfaces;

namespace Taller_TIC1_Backend.Repositories
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly ApplicationDbContext _context;

        public EmpleadoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Empleado>> GetAllAsync()
        {
            return await _context.Empleados
                .Include(emp => emp.TipoEmpleado)
                .ToListAsync();
        }

        public async Task<Empleado?> GetByIdAsync(int id)
        {
            return await _context.Empleados
                .Include(e => e.TipoEmpleado)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Empleado> CreateAsync(Empleado empleado)
        {
            _context.Empleados.Add(empleado);
            await _context.SaveChangesAsync();
            return empleado;
        }

        public async Task<Empleado?> UpdateAsync(int id, Empleado empleado)
        {
            var existingEmpleado = await _context.Empleados.FindAsync(id);
            if (existingEmpleado == null)
                return null;

            existingEmpleado.Nombre = empleado.Nombre;
            existingEmpleado.Apellido = empleado.Apellido;
            existingEmpleado.Cedula = empleado.Cedula;
            existingEmpleado.Salario = empleado.Salario;
            existingEmpleado.IdTipoEmpleado = empleado.IdTipoEmpleado; 

            await _context.SaveChangesAsync();
            return existingEmpleado;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
                return false;

            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Empleados.AnyAsync(e => e.Id == id);
        }

        public async Task<Empleado?> GetByCedulaAsync(long cedula)
        {
            return await _context.Empleados
                .Include(e => e.TipoEmpleado)
                .FirstOrDefaultAsync(e => e.Cedula == cedula);
        }
    }
}
