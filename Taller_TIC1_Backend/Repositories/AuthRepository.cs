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

    }
}
