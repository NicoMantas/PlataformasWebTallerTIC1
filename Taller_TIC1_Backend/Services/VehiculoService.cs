using Taller_TIC1_Backend.Models;
using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Repositories.Interfaces;
using Taller_TIC1_Backend.Services.Interfaces;

namespace Taller_TIC1_Backend.Services
{
    public class VehiculoService : IVehiculoService
    {
        private readonly IVehiculoRepository _vehiculoRepository;

        public VehiculoService(IVehiculoRepository vehiculoRepository)
        {
            _vehiculoRepository = vehiculoRepository;
        }

        public async Task<IEnumerable<VehiculoResponseDto>> GetAllAsync()
        {
            var vehiculos = await _vehiculoRepository.GetAllAsync();
            return vehiculos.Select(MapToResponseDto);
        }

        public async Task<VehiculoResponseDto?> GetByIdAsync(int id)
        {
            var vehiculo = await _vehiculoRepository.GetByIdAsync(id);
            return vehiculo != null ? MapToResponseDto(vehiculo) : null;
        }

        public async Task<VehiculoResponseDto> CreateAsync(VehiculoCreateDto vehiculoCreateDto)
        {
            var vehiculo = new Vehiculo
            {
                Placa = vehiculoCreateDto.Placa,
                Marca = vehiculoCreateDto.Marca,
                Modelo = vehiculoCreateDto.Modelo,
                Año = vehiculoCreateDto.Año
            };

            var createdVehiculo = await _vehiculoRepository.CreateAsync(vehiculo);
            return MapToResponseDto(createdVehiculo);
        }

        public async Task<VehiculoResponseDto?> UpdateAsync(int id, VehiculoCreateDto vehiculoUpdateDto)
        {
            var vehiculo = new Vehiculo
            {
                Id = id,
                Placa = vehiculoUpdateDto.Placa,
                Marca = vehiculoUpdateDto.Marca,
                Modelo = vehiculoUpdateDto.Modelo,
                Año = vehiculoUpdateDto.Año
            };

            var updatedVehiculo = await _vehiculoRepository.UpdateAsync(id, vehiculo);
            return updatedVehiculo != null ? MapToResponseDto(updatedVehiculo) : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _vehiculoRepository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _vehiculoRepository.ExistsAsync(id);
        }

        public async Task<VehiculoResponseDto?> GetByPlacaAsync(string placa)
        {
            var vehiculo = await _vehiculoRepository.GetByPlacaAsync(placa);
            return vehiculo != null ? MapToResponseDto(vehiculo) : null;
        }

        private static VehiculoResponseDto MapToResponseDto(Vehiculo vehiculo)
        {
            return new VehiculoResponseDto
            {
                Id = vehiculo.Id,
                Placa = vehiculo.Placa,
                Marca = vehiculo.Marca,
                Modelo = vehiculo.Modelo,
                Año = vehiculo.Año,
                TipoVehiculo = "General" // Por defecto, se puede determinar según el tipo específico
            };
        }
    }
}
