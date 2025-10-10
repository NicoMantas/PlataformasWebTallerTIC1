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

        public async Task<bool> DesactivarConDetallesAsync(int id, string detallesDesactivacion)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
                return false;

            empleado.Activo = false;
            empleado.DetallesDesactivacion = detallesDesactivacion;
            empleado.FechaDesactivacion = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActivateAsync(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
                return false;

            empleado.Activo = true;
            empleado.DetallesDesactivacion = null; // Limpiar detalles al reactivar
            empleado.FechaDesactivacion = null;
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
                TipoEmpleadoDescripcion = empleado.TipoEmpleado?.Descripcion
            };
        }
    }
}
