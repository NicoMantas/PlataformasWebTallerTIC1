using Microsoft.EntityFrameworkCore;
using Taller_TIC1_Backend.Data;
using Taller_TIC1_Backend.Models;
using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Repositories.Interfaces;
using Taller_TIC1_Backend.Services.Interfaces;

namespace Taller_TIC1_Backend.Services
{
    public class EmpleadoService : IEmpleadoService
    {
        private readonly IEmpleadoRepository _empleadoRepository;
        private readonly ApplicationDbContext _context;

        public EmpleadoService(IEmpleadoRepository empleadoRepository, ApplicationDbContext context)
        {
            _empleadoRepository = empleadoRepository;
            _context = context;
        }



        private async Task<int> GetNextIdAsync()
        {
            var empleados = await _context.Empleados.ToListAsync();
            if (!empleados.Any())
                return 1;

            return empleados.Max(t => t.Id) + 1;
        }

        public async Task<IEnumerable<EmpleadoResponseDto>> GetAllAsync()
        {
            var empleados = await _empleadoRepository.GetAllAsync();
            return empleados.Select(MapToResponseDto);
        }

        public async Task<EmpleadoResponseDto?> GetByIdAsync(int id)
        {
            var empleado = await _empleadoRepository.GetByIdAsync(id);
            return empleado != null ? MapToResponseDto(empleado) : null;
        }

        public async Task<EmpleadoResponseDto> CreateAsync(EmpleadoCreateDto empleadoCreateDto)

        {
            var nextId = await GetNextIdAsync();

            var empleado = new Empleado
            {
                Id = nextId,
                Nombre = empleadoCreateDto.Nombre,
                Apellido = empleadoCreateDto.Apellido,
                Cedula = empleadoCreateDto.Cedula,
                Salario = empleadoCreateDto.Salario,
                FechaContratacion = DateTime.Now,
                IdTipoEmpleado = empleadoCreateDto.IdTipoEmpleado,
                Activo = true // Por defecto, los empleados nuevos están activos
            };
            _context.Empleados.Add(empleado);
            await _context.SaveChangesAsync();

            return MapToResponseDto(empleado);
        }

        public async Task<EmpleadoResponseDto?> UpdateAsync(int id, EmpleadoUpdateDto empleadoUpdateDto)
        {
            var empleado = new Empleado
            {
                Id = id,
                Nombre = empleadoUpdateDto.Nombre,
                Apellido = empleadoUpdateDto.Apellido,
                Cedula = empleadoUpdateDto.Cedula,
                Salario = empleadoUpdateDto.Salario,
                IdTipoEmpleado = empleadoUpdateDto.IdTipoEmpleado
            };

            var updatedEmpleado = await _empleadoRepository.UpdateAsync(id, empleado);
            return updatedEmpleado != null ? MapToResponseDto(updatedEmpleado) : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            // En lugar de eliminar físicamente, marcar como inactivo
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
                return false;

            empleado.Activo = false;
            empleado.FechaDesactivacion = DateTime.Now;
            // Los detalles se establecerán desde el controlador con el DTO
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DesactivarConDetallesAsync(int id, string detallesDesactivacion, DateTime? fechaDesactivacionPersonalizada = null, DateTime? fechaActivacionPersonalizada = null)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
                return false;

            // Convertir fechas UTC a fechas locales para PostgreSQL
            var fechaDesactivacion = fechaDesactivacionPersonalizada?.Kind == DateTimeKind.Utc 
                ? DateTime.SpecifyKind(fechaDesactivacionPersonalizada.Value.ToLocalTime(), DateTimeKind.Unspecified)
                : fechaDesactivacionPersonalizada ?? DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);

            // Validar que la fecha de desactivación sea posterior a la fecha de contratación
            if (fechaDesactivacion < empleado.FechaContratacion)
            {
                throw new ArgumentException("La fecha de desactivación no puede ser anterior a la fecha de contratación.");
            }

            // Si hay una fecha de activación previa, validar que la desactivación sea posterior
            if (empleado.FechaActivacion.HasValue && fechaDesactivacion <= empleado.FechaActivacion.Value)
            {
                throw new ArgumentException("La fecha de desactivación debe ser posterior a la fecha de activación más reciente.");
            }

            empleado.Activo = false;
            empleado.DetallesDesactivacion = detallesDesactivacion;
            empleado.FechaDesactivacion = fechaDesactivacion;
            
            // Si se proporciona una fecha de activación futura, guardarla
            if (fechaActivacionPersonalizada.HasValue)
            {
                // Convertir fecha de activación UTC a local
                var fechaActivacion = fechaActivacionPersonalizada.Value.Kind == DateTimeKind.Utc 
                    ? DateTime.SpecifyKind(fechaActivacionPersonalizada.Value.ToLocalTime(), DateTimeKind.Unspecified)
                    : fechaActivacionPersonalizada.Value;
                
                // Validar que la fecha de activación sea posterior a la fecha de desactivación
                if (fechaActivacion <= fechaDesactivacion)
                {
                    throw new ArgumentException("La fecha de activación debe ser posterior a la fecha de desactivación.");
                }
                empleado.FechaActivacion = fechaActivacion;
            }
            else
            {
                empleado.FechaActivacion = null; // Limpiar fecha de activación si no se especifica
            }
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActivateAsync(int id, DateTime? fechaActivacionPersonalizada = null)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
                return false;

            // Convertir fecha UTC a local para PostgreSQL
            var fechaActivacion = fechaActivacionPersonalizada?.Kind == DateTimeKind.Utc 
                ? DateTime.SpecifyKind(fechaActivacionPersonalizada.Value.ToLocalTime(), DateTimeKind.Unspecified)
                : fechaActivacionPersonalizada ?? DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);

            // Validar que la fecha de activación sea posterior a la fecha de contratación
            if (fechaActivacion < empleado.FechaContratacion)
            {
                throw new ArgumentException("La fecha de activación no puede ser anterior a la fecha de contratación.");
            }

            // Si hay una fecha de desactivación previa, validar que la activación sea posterior
            if (empleado.FechaDesactivacion.HasValue && fechaActivacion <= empleado.FechaDesactivacion.Value)
            {
                throw new ArgumentException("La fecha de activación debe ser posterior a la fecha de desactivación más reciente.");
            }

            empleado.Activo = true;
            empleado.DetallesDesactivacion = null; // Limpiar detalles al reactivar
            empleado.FechaDesactivacion = null; // Limpiar fecha de desactivación al activar
            empleado.FechaActivacion = fechaActivacion; // Registrar fecha de activación
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _empleadoRepository.ExistsAsync(id);
        }

        public async Task<EmpleadoResponseDto?> GetByCedulaAsync(long cedula)
        {
            var empleado = await _empleadoRepository.GetByCedulaAsync(cedula);
            return empleado != null ? MapToResponseDto(empleado) : null;
        }

        private static EmpleadoResponseDto MapToResponseDto(Empleado empleado)
        {
            return new EmpleadoResponseDto
            {
                Id = empleado.Id,
                Nombre = empleado.Nombre,
                Apellido = empleado.Apellido,
                Cedula = empleado.Cedula,
                Salario = empleado.Salario,
                FechaContratacion = empleado.FechaContratacion,
                IdTipoEmpleado = empleado.IdTipoEmpleado,
                Activo = empleado.Activo,
                DetallesDesactivacion = empleado.DetallesDesactivacion,
                FechaDesactivacion = empleado.FechaDesactivacion,
                FechaActivacion = empleado.FechaActivacion,
                TipoEmpleadoDescripcion = empleado.TipoEmpleado?.Descripcion
            };
        }
    }
}
