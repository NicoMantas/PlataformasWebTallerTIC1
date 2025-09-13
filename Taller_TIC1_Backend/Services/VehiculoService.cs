using Microsoft.EntityFrameworkCore;
using Taller_TIC1_Backend.Data;
using Taller_TIC1_Backend.Models;
using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Repositories.Interfaces;
using Taller_TIC1_Backend.Services.Interfaces;

namespace Taller_TIC1_Backend.Services
{
    public class VehiculoService : IVehiculoService
    {
        private readonly IVehiculoRepository _vehiculoRepository;
        private readonly ApplicationDbContext _context;

        public VehiculoService(IVehiculoRepository vehiculoRepository, ApplicationDbContext context) // Inyección de dependencias
        {
            _vehiculoRepository = vehiculoRepository;
            _context = context;
        }

        public async Task<IEnumerable<VehiculoResponseDto>> GetAllAsync()
        {
            var vehiculos = await _vehiculoRepository.GetAllAsync();
            return vehiculos.Select(MapToResponseDto);
        }

        public async Task<VehiculoResponseDto?> GetByIdAsync(int id)
        {
            var vehiculo = await _context.Vehiculos
                .Include(v => v.VGasolina)
                .Include(v => v.VElectrico)
                .Include(v => v.VHibrido)
                .FirstOrDefaultAsync(v => v.Id == id);

            return vehiculo != null ? MapToResponseDto(vehiculo) : null;
        }

        // Método para obtener el próximo ID disponible
        private async Task<int> GetNextIdAsync()
        {
            var allVehiculos = await _context.Vehiculos.ToListAsync();
            if (!allVehiculos.Any())
                return 1;

            return allVehiculos.Max(v => v.Id) + 1;
        }

        // Crear un vehículo junto con su subtipo

        public async Task<VehiculoResponseDto> CreateAsync(VehiculoCreateDto vehiculoCreateDto)
        {
            try
            {
                // Obtener el próximo ID disponible
                var nextId = await GetNextIdAsync();

                // Crear el vehículo base con el ID generado manualmente
                var vehiculo = new Vehiculo
                {
                    Id = nextId, // Asignar el ID manualmente
                    Placa = vehiculoCreateDto.Placa,
                    Marca = vehiculoCreateDto.Marca,
                    Modelo = vehiculoCreateDto.Modelo,
                    Anio = vehiculoCreateDto.Anio
                };

                _context.Vehiculos.Add(vehiculo);
                await _context.SaveChangesAsync();

                // Crear subtipo según el tipo especificado
                if (!string.IsNullOrEmpty(vehiculoCreateDto.Tipo))
                {
                    switch (vehiculoCreateDto.Tipo.ToLower())
                    {
                        case "gasolina":
                            if (vehiculoCreateDto.Cilindraje.HasValue)
                            {
                                var vGasolina = new VGasolina
                                {
                                    IdVehiculo = vehiculo.Id, // Usar el ID generado manualmente
                                    Cilindraje = vehiculoCreateDto.Cilindraje.Value
                                };
                                _context.VehiculosGasolina.Add(vGasolina);
                            }
                            break;

                        case "electrico":
                            if (vehiculoCreateDto.CapacidadBateria.HasValue)
                            {
                                var vElectrico = new VElectrico
                                {
                                    IdVehiculo = vehiculo.Id, // Usar el ID generado manualmente
                                    CapacidadBateria = vehiculoCreateDto.CapacidadBateria.Value
                                };
                                _context.VehiculosElectricos.Add(vElectrico);
                            }
                            break;

                        case "hibrido":
                            if (vehiculoCreateDto.Cilindraje.HasValue && vehiculoCreateDto.CapacidadBateria.HasValue)
                            {
                                var vHibrido = new VHibrido
                                {
                                    IdVehiculo = vehiculo.Id, // Usar el ID generado manualmente
                                    Cilindraje = vehiculoCreateDto.Cilindraje.Value,
                                    CapacidadBateria = vehiculoCreateDto.CapacidadBateria.Value
                                };
                                _context.VehiculosHibridos.Add(vHibrido);
                            }
                            break;
                    }

                    await _context.SaveChangesAsync();
                }

                // Recargar el vehículo con las relaciones
                var vehiculoCompleto = await _context.Vehiculos
                    .Include(v => v.VGasolina)
                    .Include(v => v.VElectrico)
                    .Include(v => v.VHibrido)
                    .FirstOrDefaultAsync(v => v.Id == vehiculo.Id);

                return MapToResponseDto(vehiculoCompleto!);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear el vehículo: {ex.Message}", ex);
            }
        }



        public async Task<VehiculoResponseDto?> UpdateAsync(int id, VehiculoCreateDto vehiculoUpdateDto)
        {
            var vehiculo = new Vehiculo
            {
                Id = id,
                Placa = vehiculoUpdateDto.Placa,
                Marca = vehiculoUpdateDto.Marca,
                Modelo = vehiculoUpdateDto.Modelo,
                Anio = vehiculoUpdateDto.Anio
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
            var response = new VehiculoResponseDto
            {
                Id = vehiculo.Id,
                Placa = vehiculo.Placa,
                Marca = vehiculo.Marca,
                Modelo = vehiculo.Modelo,
                Anio = vehiculo.Anio,
                TipoVehiculo = vehiculo.TipoVehiculo
            };

            // Agregar propiedades específicas según el tipo
            if (vehiculo.VGasolina != null)
            {
                response.Cilindraje = vehiculo.VGasolina.Cilindraje;
            }
            else if (vehiculo.VElectrico != null)
            {
                response.CapacidadBateria = vehiculo.VElectrico.CapacidadBateria;
            }
            else if (vehiculo.VHibrido != null)
            {
                response.Cilindraje = vehiculo.VHibrido.Cilindraje;
                response.CapacidadBateria = vehiculo.VHibrido.CapacidadBateria;
            }

            return response;
        }
    }
}
