using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Models;
using Taller_TIC1_Backend.Repositories.Interfaces;
using Taller_TIC1_Backend.Services.Interfaces;

namespace Taller_TIC1_Backend.Services
{
    public class RepuestoProveedorService : IRepuestoProveedorService
    {
        private readonly IRepuestoProveedorRepository _repository;
        private readonly IRepuestoRepository _repuestoRepository;
        private readonly IProveedorRepository _proveedorRepository;

        public RepuestoProveedorService(
            IRepuestoProveedorRepository repository,
            IRepuestoRepository repuestoRepository,
            IProveedorRepository proveedorRepository)
        {
            _repository = repository;
            _repuestoRepository = repuestoRepository;
            _proveedorRepository = proveedorRepository;
        }

        public async Task<IEnumerable<RepuestoProveedorResponseDto>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return items.Select(MapToResponseDto);
        }

        public async Task<RepuestoProveedorResponseDto?> GetByIdAsync(int idRepuesto, int idProveedor)
        {
            var entity = await _repository.GetByIdAsync(idRepuesto, idProveedor);
            return entity != null ? MapToResponseDto(entity) : null;
        }

        public async Task<RepuestoProveedorResponseDto> CreateAsync(RepuestoProveedorCreateDto dto)
        {
            // Verificar que existan tanto el repuesto como el proveedor
            var repuestoExists = await _repuestoRepository.ExistsAsync(dto.IdRepuesto);
            var proveedorExists = await _proveedorRepository.ExistsAsync(dto.IdProveedor);

            if (!repuestoExists || !proveedorExists)
                throw new ArgumentException("El repuesto o el proveedor no existen");

            // Verificar que la relación no exista ya
            var exists = await _repository.ExistsAsync(dto.IdRepuesto, dto.IdProveedor);
            if (exists)
                throw new InvalidOperationException("Esta relación ya existe");

            var entity = new RepuestoProveedor
            {
                IdRepuesto = dto.IdRepuesto,
                IdProveedor = dto.IdProveedor
            };

            var created = await _repository.CreateAsync(entity);
            return MapToResponseDto(created);
        }

        public Task<bool> DeleteAsync(int idRepuesto, int idProveedor) =>
            _repository.DeleteAsync(idRepuesto, idProveedor);

        public async Task<IEnumerable<RepuestoProveedorResponseDto>> GetByRepuestoIdAsync(int idRepuesto)
        {
            var items = await _repository.GetByRepuestoIdAsync(idRepuesto);
            return items.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<RepuestoProveedorResponseDto>> GetByProveedorIdAsync(int idProveedor)
        {
            var items = await _repository.GetByProveedorIdAsync(idProveedor);
            return items.Select(MapToResponseDto);
        }

        private static RepuestoProveedorResponseDto MapToResponseDto(RepuestoProveedor repuestoProveedor)
        {
            return new RepuestoProveedorResponseDto
            {
                IdRepuesto = repuestoProveedor.IdRepuesto,
                IdProveedor = repuestoProveedor.IdProveedor,
                Repuesto = repuestoProveedor.Repuesto != null ? new RepuestoResponseDto
                {
                    Id = repuestoProveedor.Repuesto.Id,
                    Nombre = repuestoProveedor.Repuesto.Nombre,
                    NumeroSerie = repuestoProveedor.Repuesto.Numero_serie,
                    Precio = repuestoProveedor.Repuesto.Precio,
                    Stock = repuestoProveedor.Repuesto.Stock
                } : null,
                Proveedor = repuestoProveedor.Proveedor != null ? new ProveedorResponseDto
                {
                    Id = repuestoProveedor.Proveedor.Id,
                    Nombre = repuestoProveedor.Proveedor.Nombre,
                    Contacto = repuestoProveedor.Proveedor.Contacto
                } : null
            };
        }
    }
}
