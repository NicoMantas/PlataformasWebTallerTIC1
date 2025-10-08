using Taller_TIC1_Backend.Data;
using Taller_TIC1_Backend.Models;
using Taller_TIC1_Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Taller_TIC1_Backend.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        //implementacion de la interfaz\
        private readonly ApplicationDbContext _context;

        public AuthRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UsuarioEmpleadoTaller?> GetUsuarioEmpleadoByEmailAsync(string email)
        {
            return await _context.UsuariosEmpleadoTaller
                .Include(u => u.Empleado)
                .Include(u => u.Taller)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<UsuarioClienteTaller?> GetUsuarioClienteByEmailAsync(string email)
        {
            return await _context.UsuariosClienteTaller
                .Include(u => u.Cliente)
                .ThenInclude(c => c.Vehiculo)
                .Include(u => u.Taller)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<UsuarioEmpleadoTaller> CreateUsuarioEmpleadoAsync(UsuarioEmpleadoTaller usuario)
        {
            _context.UsuariosEmpleadoTaller.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<UsuarioClienteTaller> CreateUsuarioClienteAsync(UsuarioClienteTaller usuario)
        {
            _context.UsuariosClienteTaller.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.UsuariosEmpleadoTaller.AnyAsync(u => u.Email == email) ||
                   await _context.UsuariosClienteTaller.AnyAsync(u => u.Email == email);
        }

        // CRUD para UsuarioEmpleadoTaller
        public async Task<IEnumerable<UsuarioEmpleadoTaller>> GetAllUsuariosEmpleadoAsync()
        {
            return await _context.UsuariosEmpleadoTaller
                .Include(u => u.Empleado)
                .Include(u => u.Taller)
                .ToListAsync();
        }

        public async Task<UsuarioEmpleadoTaller?> GetUsuarioEmpleadoByIdAsync(int id)
        {
            return await _context.UsuariosEmpleadoTaller
                .Include(u => u.Empleado)
                .Include(u => u.Taller)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateUsuarioEmpleadoAsync(UsuarioEmpleadoTaller usuario)
        {
            _context.UsuariosEmpleadoTaller.Update(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUsuarioEmpleadoAsync(int id)
        {
            var usuario = await _context.UsuariosEmpleadoTaller.FindAsync(id);
            if (usuario != null)
            {
                _context.UsuariosEmpleadoTaller.Remove(usuario);
                await _context.SaveChangesAsync();
            }
        }

        // CRUD para UsuarioClienteTaller
        public async Task<IEnumerable<UsuarioClienteTaller>> GetAllUsuariosClienteAsync()
        {
            return await _context.UsuariosClienteTaller
                .Include(u => u.Cliente)
                .ThenInclude(c => c.Vehiculo)
                .Include(u => u.Taller)
                .ToListAsync();
        }

        public async Task<UsuarioClienteTaller?> GetUsuarioClienteByIdAsync(int id)
        {
            return await _context.UsuariosClienteTaller
                .Include(u => u.Cliente)
                .ThenInclude(c => c.Vehiculo)
                .Include(u => u.Taller)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateUsuarioClienteAsync(UsuarioClienteTaller usuario)
        {
            _context.UsuariosClienteTaller.Update(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUsuarioClienteAsync(int id)
        {
            var usuario = await _context.UsuariosClienteTaller.FindAsync(id);
            if (usuario != null)
            {
                _context.UsuariosClienteTaller.Remove(usuario);
                await _context.SaveChangesAsync();
            }
        }

    }
}
