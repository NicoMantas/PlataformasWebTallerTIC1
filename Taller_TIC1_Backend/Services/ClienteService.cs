using Taller_TIC1_Backend.Models;
using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Repositories.Interfaces;
using Taller_TIC1_Backend.Services.Interfaces;

namespace Taller_TIC1_Backend.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<IEnumerable<ClienteResponseDto>> GetAllAsync()
        {
            var clientes = await _clienteRepository.GetAllAsync();
            return clientes.Select(MapToResponseDto);
        }

        public async Task<ClienteResponseDto?> GetByIdAsync(int id)
        {
            var cliente = await _clienteRepository.GetByIdAsync(id);
            return cliente != null ? MapToResponseDto(cliente) : null;
        }

        public async Task<ClienteResponseDto> CreateAsync(ClienteCreateDto clienteCreateDto)
        {
            var cliente = new Cliente
            {
                Nombre = clienteCreateDto.Nombre,
                Email = clienteCreateDto.Email,
                Telefono = clienteCreateDto.Telefono,
                IdVehiculo = clienteCreateDto.IdVehiculo
            };

            var createdCliente = await _clienteRepository.CreateAsync(cliente);
            return MapToResponseDto(createdCliente);
        }

        public async Task<ClienteResponseDto?> UpdateAsync(int id, ClienteUpdateDto clienteUpdateDto)
        {
            var cliente = new Cliente
            {
                Id = id,
                Nombre = clienteUpdateDto.Nombre,
                Email = clienteUpdateDto.Email,
                Telefono = clienteUpdateDto.Telefono,
                IdVehiculo = clienteUpdateDto.IdVehiculo
            };

            var updatedCliente = await _clienteRepository.UpdateAsync(id, cliente);
            return updatedCliente != null ? MapToResponseDto(updatedCliente) : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _clienteRepository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _clienteRepository.ExistsAsync(id);
        }

        private static ClienteResponseDto MapToResponseDto(Cliente cliente)
        {
            return new ClienteResponseDto
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Email = cliente.Email,
                Telefono = cliente.Telefono,
                IdVehiculo = cliente.IdVehiculo,
                VehiculoInfo = cliente.Vehiculo != null 
                    ? $"{cliente.Vehiculo.Marca} {cliente.Vehiculo.Modelo} - {cliente.Vehiculo.Placa}"
                    : null
            };
        }
    }
}
