using Microsoft.EntityFrameworkCore;
using Taller_TIC1_Backend.Data;
using Taller_TIC1_Backend.Models;
using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Repositories.Interfaces;
using Taller_TIC1_Backend.Services.Interfaces;

namespace Taller_TIC1_Backend.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly ApplicationDbContext _context;

        public ClienteService(IClienteRepository clienteRepository, ApplicationDbContext context) //constructor
        {
            _clienteRepository = clienteRepository;
            _context = context;
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


        //create
        public async Task<ClienteResponseDto> CreateAsync(ClienteCreateDto clienteCreateDto) // crear cliente
        {

            try
            {
                // Verificar que el veh�culo existe
                var vehiculo = await _context.Vehiculos.FindAsync(clienteCreateDto.IdVehiculo);
                if (vehiculo == null)
                {
                    throw new Exception($"El veh�culo con ID {clienteCreateDto.IdVehiculo} no existe");
                }

                // Obtener el pr�ximo ID disponible
                var nextId = await GetNextIdAsync();

                // Crear el cliente base
                var cliente = new Cliente
                {
                    Id = nextId,
                    Nombre = clienteCreateDto.Nombre,
                    Email = clienteCreateDto.Email,
                    Telefono = clienteCreateDto.Telefono,
                    IdVehiculo = clienteCreateDto.IdVehiculo,
                    Activo = true // Por defecto, los clientes nuevos están activos
                };

                // Asegurar que Activo esté explícitamente asignado
                cliente.Activo = true;
                
                Console.WriteLine($"[ClienteService] Creando cliente con Activo = {cliente.Activo}");

                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();
                
                Console.WriteLine($"[ClienteService] Cliente guardado con ID: {cliente.Id}, Activo: {cliente.Activo}");

                // Crear subtipo seg�n el tipo especificado
                if (!string.IsNullOrEmpty(clienteCreateDto.Tipo))
                {
                    switch (clienteCreateDto.Tipo.ToLower())
                    {
                        case "natural":
                            // Validar campos requeridos para cliente natural
                            if (!clienteCreateDto.Cedula.HasValue)
                            {
                                throw new Exception("La cédula es requerida para cliente natural");
                            }
                            
                            if (string.IsNullOrEmpty(clienteCreateDto.Apellido))
                            {
                                throw new Exception("El apellido es requerido para cliente natural");
                            }

                            var cNatural = new CNatural
                            {
                                IdCliente = cliente.Id,
                                Cedula = clienteCreateDto.Cedula.Value,
                                Apellido = clienteCreateDto.Apellido
                            };
                            _context.ClientesNaturales.Add(cNatural);
                            break;

                        case "empresa":
                            // Validar campos espec�ficos para empresa
                            if (!clienteCreateDto.Nit.HasValue || clienteCreateDto.Nit.Value == 0)
                            {
                                throw new Exception("El NIT es requerido y debe ser mayor a 0 para cliente empresa");
                            }

                            if (string.IsNullOrEmpty(clienteCreateDto.RepresentanteLegal))
                            {
                                throw new Exception("El representante legal es requerido para cliente empresa");
                            }

                            var cEmpresa = new CEmpresa
                            {
                                IdCliente = cliente.Id,
                                Nit = clienteCreateDto.Nit.Value,
                                RepresentanteLegal = clienteCreateDto.RepresentanteLegal
                            };
                            _context.ClientesEmpresa.Add(cEmpresa);
                            break;
                    }

                    await _context.SaveChangesAsync();
                }

                return await GetByIdAsync(cliente.Id);

            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear el cliente: {ex.Message}", ex);
            }
        }

        private async Task<int> GetNextIdAsync()
        {
            var allClientes = await _context.Clientes.ToListAsync();
            if (!allClientes.Any())
                return 1;

            return allClientes.Max(c => c.Id) + 1;
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
            // En lugar de eliminar físicamente, marcar como inactivo
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return false;

            cliente.Activo = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActivateAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return false;

            cliente.Activo = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _clienteRepository.ExistsAsync(id);
        }

        private static ClienteResponseDto MapToResponseDto(Cliente cliente)
        {
            var response = new ClienteResponseDto
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Email = cliente.Email,
                Telefono = cliente.Telefono,
                IdVehiculo = cliente.IdVehiculo,
                Activo = cliente.Activo,
                TipoCliente = cliente.TipoCliente,
                VehiculoInfo = cliente.Vehiculo != null
                    ? $"{cliente.Vehiculo.Marca} {cliente.Vehiculo.Modelo} - {cliente.Vehiculo.Placa}"
                    : null
            };

            // Agregar propiedades espec�ficas seg�n el tipo
            if (cliente.CNatural != null)
            {
                response.Cedula = cliente.CNatural.Cedula;
                response.Apellido = cliente.CNatural.Apellido;
            }
            else if (cliente.CEmpresa != null)
            {
                response.Nit = cliente.CEmpresa.Nit;
                response.RepresentanteLegal = cliente.CEmpresa.RepresentanteLegal;
            }

            return response;
        }
    }
}
