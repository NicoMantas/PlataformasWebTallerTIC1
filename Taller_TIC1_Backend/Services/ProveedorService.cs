using Taller_TIC1_Backend.Models;
using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Repositories.Interfaces;
using Taller_TIC1_Backend.Services.Interfaces;

namespace Taller_TIC1_Backend.Services
{
    public class ProveedorService : IProveedorService
    {
        private readonly IProveedorRepository _repository;

        public ProveedorService(IProveedorRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProveedorResponseDto>> GetAllAsync()
        {
            var proveedores = await _repository.GetAllAsync();
            return proveedores.Select(MapToResponseDto);
        }

        public async Task<ProveedorResponseDto?> GetByIdAsync(int id)
        {
            var proveedor = await _repository.GetByIdAsync(id);
            return proveedor != null ? MapToResponseDto(proveedor) : null;
        }

        public async Task<ProveedorResponseDto> CreateAsync(ProveedorCreateDto dto)
        {
            // Generar el siguiente ID disponible
            var nextId = await GetNextIdAsync();
            
            var proveedor = new Proveedor
            {
                Id = nextId,
                Nombre = dto.Nombre,
                Contacto = dto.Contacto ?? string.Empty
            };

            var createdProveedor = await _repository.CreateAsync(proveedor);
            return MapToResponseDto(createdProveedor);
        }

        public async Task<ProveedorResponseDto?> UpdateAsync(ProveedorUpdateDto dto)
        {
            var proveedor = new Proveedor
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                Contacto = dto.Contacto ?? string.Empty
            };

            var updatedProveedor = await _repository.UpdateAsync(proveedor);
            return updatedProveedor != null ? MapToResponseDto(updatedProveedor) : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ProveedorResponseDto>> SearchByNameAsync(string name)
        {
            var proveedores = await _repository.SearchByNameAsync(name);
            return proveedores.Select(MapToResponseDto);
        }

        private async Task<int> GetNextIdAsync()
        {
            var allProveedores = await _repository.GetAllAsync();
            if (!allProveedores.Any())
                return 1;
            
            return allProveedores.Max(p => p.Id) + 1;
        }

        private static ProveedorResponseDto MapToResponseDto(Proveedor proveedor)
        {
            return new ProveedorResponseDto
            {
                Id = proveedor.Id,
                Nombre = proveedor.Nombre,
                Contacto = proveedor.Contacto
            };
        }
    }
}
