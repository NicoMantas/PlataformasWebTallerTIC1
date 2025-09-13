using Taller_TIC1_Backend.Models;
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
            // Generar el siguiente ID disponible
            var nextId = await GetNextIdAsync();
            
            var entity = new Repuesto
            {
                Id = nextId,
                Nombre = dto.Nombre,
                Numero_serie = dto.NumeroSerie ?? 0,
                Precio = dto.Precio,
                Stock = dto.Stock
            };
            var created = await _repository.CreateAsync(entity);
            return MapToResponseDto(created);
        }

        public async Task<RepuestoResponseDto?> UpdateAsync(RepuestoUpdateDto dto)
        {
            var entity = new Repuesto
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                Numero_serie = dto.NumeroSerie ?? 0,
                Precio = dto.Precio,
                Stock = dto.Stock
            };
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

        private async Task<int> GetNextIdAsync()
        {
            var allRepuestos = await _repository.GetAllAsync();
            if (!allRepuestos.Any())
                return 1;
            
            return allRepuestos.Max(r => r.Id) + 1;
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
                Proveedores = new List<ProveedorResponseDto>()
            };
        }
    }
}

