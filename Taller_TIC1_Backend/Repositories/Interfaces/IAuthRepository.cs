using Taller_TIC1_Backend.Models;

namespace Taller_TIC1_Backend.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        //metodos implementados por el repositorio para consultas a la base de datos
        Task<UsuarioEmpleadoTaller?> GetUsuarioEmpleadoByEmailAsync(string email); // Nuevo método para obtener usuario empleado por email
        Task<UsuarioClienteTaller?> GetUsuarioClienteByEmailAsync(string email); // Nuevo método para obtener usuario cliente por email
        Task<UsuarioEmpleadoTaller> CreateUsuarioEmpleadoAsync(UsuarioEmpleadoTaller usuario); // Nuevo método para crear usuario empleado
        Task<UsuarioClienteTaller> CreateUsuarioClienteAsync(UsuarioClienteTaller usuario); // Nuevo método para crear usuario cliente
        Task<bool> EmailExistsAsync(string email); // Nuevo método para verificar si el email ya existe


    }
}
