using System.Security.Cryptography;
using System.Text;
using Taller_TIC1_Backend.Models;
using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Repositories.Interfaces;
using Taller_TIC1_Backend.Services.Interfaces;

namespace Taller_TIC1_Backend.Services
{
    public class AuthService : IAuthService // Implementación del servicio de autenticación
    {
        // Constructor que inyecta los repositorios necesarios
        private readonly IAuthRepository _authRepository;
        private readonly IEmpleadoRepository _empleadoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly ITallerRepository _tallerRepository;

        public AuthService( // Inyección de dependencias
            IAuthRepository authRepository,
            IEmpleadoRepository empleadoRepository,
            IClienteRepository clienteRepository,
            ITallerRepository tallerRepository)
        {
            _authRepository = authRepository;
            _empleadoRepository = empleadoRepository;
            _clienteRepository = clienteRepository;
            _tallerRepository = tallerRepository;
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
                            InfoEspecifica = empleado
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
                            InfoEspecifica = cliente
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
