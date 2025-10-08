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
                IdTipoEmpleado = empleadoCreateDto.IdTipoEmpleado
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
            return await _empleadoRepository.DeleteAsync(id);
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
                TipoEmpleadoDescripcion = empleado.TipoEmpleado?.Descripcion
            };
        }
    }
}
