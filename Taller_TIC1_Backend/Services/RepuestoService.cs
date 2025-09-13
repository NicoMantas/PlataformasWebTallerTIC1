using B_TallerAutomoviles.Clases;
using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Repositories.Interfaces;
using Taller_TIC1_Backend.Services.Interfaces;

namespace Taller_TIC1_Backend.Services
{
    public class RepuestoService : IRepuestoService
    {
        private readonly IRepuestoRepository _repository;

        public RepuestoService(IRepuestoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<RepuestoResponseDto>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return items.Select(MapToResponseDto);
        }

        public async Task<RepuestoResponseDto?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity != null ? MapToResponseDto(entity) : null;
        }

        public async Task<RepuestoResponseDto> CreateAsync(RepuestoCreateDto dto)
        {
            var entity = new Repuesto(0, dto.Nombre, dto.NumeroSerie ?? 0, dto.Precio, dto.Stock, new List<Proveedor>());
            var created = await _repository.CreateAsync(entity);
            return MapToResponseDto(created);
        }

        public async Task<RepuestoResponseDto?> UpdateAsync(RepuestoUpdateDto dto)
        {
            var entity = new Repuesto(dto.Id, dto.Nombre, dto.NumeroSerie ?? 0, dto.Precio, dto.Stock, new List<Proveedor>());
            var updated = await _repository.UpdateAsync(entity);
            return updated != null ? MapToResponseDto(updated) : null;
        }

        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);

        public async Task<IEnumerable<RepuestoResponseDto>> SearchByNameAsync(string name)
        {
            var items = await _repository.SearchByNameAsync(name);
            return items.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<RepuestoResponseDto>> GetByStockAsync(int minStock)
        {
            var items = await _repository.GetByStockAsync(minStock);
            return items.Select(MapToResponseDto);
        }

        public async Task<RepuestoResponseDto?> GetByNumeroSerieAsync(long numeroSerie)
        {
            var item = await _repository.GetByNumeroSerieAsync(numeroSerie);
            return item != null ? MapToResponseDto(item) : null;
        }

        private static RepuestoResponseDto MapToResponseDto(Repuesto repuesto)
        {
            return new RepuestoResponseDto
            {
                Id = repuesto.Id,
                Nombre = repuesto.Nombre,
                NumeroSerie = repuesto.Numero_serie,
                Precio = repuesto.Precio,
                Stock = repuesto.Stock,
                Proveedores = repuesto.Proveedores?.Select(p => new ProveedorResponseDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Contacto = p.Contacto
                }).ToList() ?? new List<ProveedorResponseDto>()
            };
        }
    }
}

