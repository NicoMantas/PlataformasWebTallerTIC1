using B_TallerAutomoviles.Clases;
using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Repositories.Interfaces;
using Taller_TIC1_Backend.Services.Interfaces;
using static Taller_TIC1_Backend.Models.DTOs.PorveedorCreateDTOcs;

namespace Taller_TIC1_Backend.Services.Interfaces
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
            var proveedor = new Proveedor(
                id: 0,
                nombre: dto.Nombre,
                contacto: dto.Contacto
            );

            var createdProveedor = await _repository.CreateAsync(proveedor);
            return MapToResponseDto(createdProveedor);
        }

        public async Task<ProveedorResponseDto?> UpdateAsync(ProveedorUpdateDto dto)
        {
            var proveedor = new Proveedor(
                id: dto.Id,
                nombre: dto.Nombre,
                contacto: dto.Contacto
            );

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

        private static ProveedorResponseDto MapToResponseDto(Proveedor proveedor)
        {
            return new ProveedorResponseDto
            {
                Id = proveedor.Id,
                Nombre = proveedor.Nombre,
                Contacto = proveedor.Contacto,
            };
        }
    }
}
