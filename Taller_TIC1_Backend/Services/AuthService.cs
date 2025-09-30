using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Taller_TIC1_Backend.Models;
using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Repositories.Interfaces;
using Taller_TIC1_Backend.Services.Interfaces;
using Taller_TIC1_Backend.Data;

namespace Taller_TIC1_Backend.Services
{
    public class AuthService : IAuthService // Implementación del servicio de autenticación
    {
        // Constructor que inyecta los repositorios necesarios
        private readonly IAuthRepository _authRepository;
        private readonly IEmpleadoRepository _empleadoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly ITallerRepository _tallerRepository;
        private readonly ApplicationDbContext _context;

        public AuthService(
             // Inyección de dependencias
             ApplicationDbContext context,
            IAuthRepository authRepository,
            IEmpleadoRepository empleadoRepository,
            IClienteRepository clienteRepository,
            ITallerRepository tallerRepository)
           
        {
            _authRepository = authRepository;
            _empleadoRepository = empleadoRepository;
            _clienteRepository = clienteRepository;
            _tallerRepository = tallerRepository;
            _context = context;

        }
        private async Task<int> GetNextIdClientesAsync()
        {
            var Auth = await _context.UsuariosClienteTaller.ToListAsync();
            if (!Auth.Any())
                return 1;

            return Auth.Max(t => t.Id) + 1;
        }

        private async Task<int> GetNextIdEmpleadoAsync()
        {
            var Auth = await _context.UsuariosEmpleadoTaller.ToListAsync();
            if (!Auth.Any())
                return 1;

            return Auth.Max(t => t.Id) + 1;
        }
        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto loginRequest)
        {
            // Buscar primero como empleado
            var usuarioEmpleado = await _authRepository.GetUsuarioEmpleadoByEmailAsync(loginRequest.Email);
            if (usuarioEmpleado != null)
            {
                if (VerifyPassword(loginRequest.Password, usuarioEmpleado.Contrasena))
                {
                    var empleado = await _empleadoRepository.GetByIdAsync(usuarioEmpleado.IdEmpleado);
                    var taller = await _tallerRepository.GetByIdAsync(usuarioEmpleado.IdTaller);

                    return new AuthResponseDto
                    {
                        Success = true,
                        Message = "Login exitoso",
                        User = new UserInfoDto
                        {
                            Id = usuarioEmpleado.Id,
                            Email = usuarioEmpleado.Email,
                            TipoUsuario = "empleado",
                            IdTaller = usuarioEmpleado.IdTaller,
                            NombreTaller = taller?.Nombre ?? string.Empty,
                            InfoEspecifica = new
                            {
                                empleado.Id,
                                empleado.Nombre,
                                empleado.Apellido,
                                empleado.Cedula,
                                empleado.Salario,
                                empleado.FechaContratacion,
                                empleado.IdTipoEmpleado,
                                tipoEmpleado = empleado.TipoEmpleado?.Descripcion ?? "Empleado"
                            }
                        }
                    };
                }
            }

            // Buscar como cliente
            var usuarioCliente = await _authRepository.GetUsuarioClienteByEmailAsync(loginRequest.Email);
            if (usuarioCliente != null)
            {
                if (VerifyPassword(loginRequest.Password, usuarioCliente.Contrasena))
                {
                    var cliente = await _clienteRepository.GetByIdAsync(usuarioCliente.IdCliente);
                    var taller = await _tallerRepository.GetByIdAsync(usuarioCliente.IdTaller);

                    return new AuthResponseDto
                    {
                        Success = true,
                        Message = "Login exitoso",
                        User = new UserInfoDto
                        {
                            Id = usuarioCliente.Id,
                            Email = usuarioCliente.Email,
                            TipoUsuario = "cliente",
                            IdTaller = usuarioCliente.IdTaller,
                            NombreTaller = taller?.Nombre ?? string.Empty,
                            InfoEspecifica = new
                            {
                                cliente.Id,
                                cliente.Nombre,
                                cliente.Apellido,
                                cliente.Cedula,
                                cliente.Telefono,
                                cliente.Direccion,
                                cliente.Email
                            }
                        }
                    };
                }
            }

            return new AuthResponseDto
            {
                Success = false,
                Message = "Credenciales inválidas"
            };
        }

        public async Task<AuthResponseDto> RegisterEmpleadoAsync(RegisterRequestDto registerRequest)
        {
            if (await _authRepository.EmailExistsAsync(registerRequest.Email))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "El email ya está registrado"
                };
            }

            if (!registerRequest.IdEmpleado.HasValue)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "ID de empleado es requerido"
                };
            }

            var empleado = await _empleadoRepository.GetByIdAsync(registerRequest.IdEmpleado.Value);
            if (empleado == null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Empleado no encontrado"
                };
            }

            var taller = await _tallerRepository.GetByIdAsync(registerRequest.IdTaller);
            if (taller == null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Taller no encontrado"
                };
            }

            var usuario = new UsuarioEmpleadoTaller
            {
                Id = await GetNextIdEmpleadoAsync(),
                Email = registerRequest.Email,
                Contrasena = HashPassword(registerRequest.Password),
                IdTaller = registerRequest.IdTaller,
                IdEmpleado = registerRequest.IdEmpleado.Value
            };

            var createdUsuario = await _authRepository.CreateUsuarioEmpleadoAsync(usuario);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Registro exitoso",
                User = new UserInfoDto
                {
                    Id = createdUsuario.Id,
                    Email = createdUsuario.Email,
                    TipoUsuario = "empleado",
                    IdTaller = createdUsuario.IdTaller,
                    NombreTaller = taller.Nombre,
                    InfoEspecifica = empleado
                }
            };
        }

        public async Task<AuthResponseDto> RegisterClienteAsync(RegisterRequestDto registerRequest)
        {
            if (await _authRepository.EmailExistsAsync(registerRequest.Email))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "El email ya está registrado"
                };
            }

            if (!registerRequest.IdCliente.HasValue)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "ID de cliente es requerido"
                };
            }

            var cliente = await _clienteRepository.GetByIdAsync(registerRequest.IdCliente.Value);
            if (cliente == null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Cliente no encontrado"
                };
            }

            var taller = await _tallerRepository.GetByIdAsync(registerRequest.IdTaller);
            if (taller == null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Taller no encontrado"
                };
            }

            var usuario = new UsuarioClienteTaller
            {
                Id = await GetNextIdClientesAsync(),
                Email = registerRequest.Email,
                Contrasena = HashPassword(registerRequest.Password),
                IdTaller = registerRequest.IdTaller,
                IdCliente = registerRequest.IdCliente.Value
            };

            var createdUsuario = await _authRepository.CreateUsuarioClienteAsync(usuario);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Registro exitoso",
                User = new UserInfoDto
                {
                    Id = createdUsuario.Id,
                    Email = createdUsuario.Email,
                    TipoUsuario = "cliente",
                    IdTaller = createdUsuario.IdTaller,
                    NombreTaller = taller.Nombre,
                    InfoEspecifica = cliente
                }
            };
        }

        public Task<bool> EmailExistsAsync(string email)
        {
            return _authRepository.EmailExistsAsync(email);
        }

        // Nuevos métodos CRUD para UsuarioEmpleadoTaller
        public async Task<IEnumerable<UsuarioEmpleadoTaller>> GetAllUsuariosEmpleadoAsync()
        {
            return await _authRepository.GetAllUsuariosEmpleadoAsync();
        }

        public async Task<UsuarioEmpleadoTaller?> GetUsuarioEmpleadoByIdAsync(int id)
        {
            return await _authRepository.GetUsuarioEmpleadoByIdAsync(id);
        }

        public async Task<AuthResponseDto> UpdateUsuarioEmpleadoAsync(int id, UpdateUsuarioEmpleadoDto updateDto)
        {
            var usuarioExistente = await _authRepository.GetUsuarioEmpleadoByIdAsync(id);
            if (usuarioExistente == null)
            {
                return new AuthResponseDto { Success = false, Message = "Usuario no encontrado" };
            }

            // Verificar si el email ya existe en otro usuario
            if (usuarioExistente.Email != updateDto.Email &&
                await _authRepository.EmailExistsAsync(updateDto.Email))
            {
                return new AuthResponseDto { Success = false, Message = "El email ya está en uso" };
            }

            usuarioExistente.Email = updateDto.Email;
            usuarioExistente.IdTaller = updateDto.IdTaller;
            usuarioExistente.IdEmpleado = updateDto.IdEmpleado;

            if (!string.IsNullOrEmpty(updateDto.Password))
            {
                usuarioExistente.Contrasena = HashPassword(updateDto.Password);
            }

            await _authRepository.UpdateUsuarioEmpleadoAsync(usuarioExistente);

            var empleado = await _empleadoRepository.GetByIdAsync(updateDto.IdEmpleado);
            var taller = await _tallerRepository.GetByIdAsync(updateDto.IdTaller);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Usuario actualizado exitosamente",
                User = new UserInfoDto
                {
                    Id = usuarioExistente.Id,
                    Email = usuarioExistente.Email,
                    TipoUsuario = "empleado",
                    IdTaller = usuarioExistente.IdTaller,
                    NombreTaller = taller?.Nombre ?? string.Empty,
                    InfoEspecifica = empleado
                }
            };
        }

        public async Task<bool> DeleteUsuarioEmpleadoAsync(int id)
        {
            await _authRepository.DeleteUsuarioEmpleadoAsync(id);
            return true;
        }

        // Nuevos métodos CRUD para UsuarioClienteTaller
        public async Task<IEnumerable<UsuarioClienteTaller>> GetAllUsuariosClienteAsync()
        {
            return await _authRepository.GetAllUsuariosClienteAsync();
        }

        public async Task<UsuarioClienteTaller?> GetUsuarioClienteByIdAsync(int id)
        {
            return await _authRepository.GetUsuarioClienteByIdAsync(id);
        }

        public async Task<AuthResponseDto> UpdateUsuarioClienteAsync(int id, UpdateUsuarioClienteDto updateDto)
        {
            var usuarioExistente = await _authRepository.GetUsuarioClienteByIdAsync(id);
            if (usuarioExistente == null)
            {
                return new AuthResponseDto { Success = false, Message = "Usuario no encontrado" };
            }

            if (usuarioExistente.Email != updateDto.Email &&
                await _authRepository.EmailExistsAsync(updateDto.Email))
            {
                return new AuthResponseDto { Success = false, Message = "El email ya está en uso" };
            }

            usuarioExistente.Email = updateDto.Email;
            usuarioExistente.IdTaller = updateDto.IdTaller;
            usuarioExistente.IdCliente = updateDto.IdCliente;

            if (!string.IsNullOrEmpty(updateDto.Password))
            {
                usuarioExistente.Contrasena = HashPassword(updateDto.Password);
            }

            await _authRepository.UpdateUsuarioClienteAsync(usuarioExistente);

            var cliente = await _clienteRepository.GetByIdAsync(updateDto.IdCliente);
            var taller = await _tallerRepository.GetByIdAsync(updateDto.IdTaller);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Usuario actualizado exitosamente",
                User = new UserInfoDto
                {
                    Id = usuarioExistente.Id,
                    Email = usuarioExistente.Email,
                    TipoUsuario = "cliente",
                    IdTaller = usuarioExistente.IdTaller,
                    NombreTaller = taller?.Nombre ?? string.Empty,
                    InfoEspecifica = cliente
                }
            };
        }

        public async Task<bool> DeleteUsuarioClienteAsync(int id)
        {
            await _authRepository.DeleteUsuarioClienteAsync(id);
            return true;
        }

        // Elimina la segunda definición duplicada de HashPassword en la clase AuthService.
        // Mantén solo una definición de HashPassword y una de VerifyPassword.

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }

    }
}
