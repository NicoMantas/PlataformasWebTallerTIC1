using Taller_TIC1_Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Taller_TIC1_Backend.Services
{
    public class AuthorizationService
    {
        private readonly ApplicationDbContext _context;

        public AuthorizationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsAdminAsync(int empleadoId)
        {
            var empleado = await _context.Empleados
                .Include(e => e.TipoEmpleado)
                .FirstOrDefaultAsync(e => e.Id == empleadoId && e.Activo);

            if (empleado?.TipoEmpleado == null)
                return false;

            // Verificar si el tipo de empleado es administrador o gerente
            var tipoDescripcion = empleado.TipoEmpleado.Descripcion.ToLower();
            return tipoDescripcion.Contains("admin") || 
                   tipoDescripcion.Contains("gerente") || 
                   tipoDescripcion.Contains("manager") ||
                   empleado.IdTipoEmpleado == 1; // Asumir que ID 1 es administrador
        }

        public async Task<bool> IsAdminAsync(string email)
        {
            // Buscar el empleado por email a través de UsuarioEmpleadoTaller
            var usuarioEmpleado = await _context.UsuariosEmpleadoTaller
                .Include(u => u.Empleado)
                .ThenInclude(e => e.TipoEmpleado)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuarioEmpleado?.Empleado == null || !usuarioEmpleado.Empleado.Activo)
                return false;

            var empleado = usuarioEmpleado.Empleado;
            var tipoDescripcion = empleado.TipoEmpleado?.Descripcion.ToLower() ?? "";
            
            return tipoDescripcion.Contains("admin") || 
                   tipoDescripcion.Contains("gerente") || 
                   tipoDescripcion.Contains("manager") ||
                   empleado.IdTipoEmpleado == 1; // Asumir que ID 1 es administrador
        }
    }
}
