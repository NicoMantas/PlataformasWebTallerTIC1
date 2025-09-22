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

        // Nuevos métodos CRUD para UsuarioEmpleadoTaller
        Task<IEnumerable<UsuarioEmpleadoTaller>> GetAllUsuariosEmpleadoAsync();
        Task<UsuarioEmpleadoTaller?> GetUsuarioEmpleadoByIdAsync(int id);
        Task UpdateUsuarioEmpleadoAsync(UsuarioEmpleadoTaller usuario);
        Task DeleteUsuarioEmpleadoAsync(int id);

        // Nuevos métodos CRUD para UsuarioClienteTaller
        Task<IEnumerable<UsuarioClienteTaller>> GetAllUsuariosClienteAsync();
        Task<UsuarioClienteTaller?> GetUsuarioClienteByIdAsync(int id);
        Task UpdateUsuarioClienteAsync(UsuarioClienteTaller usuario);
        Task DeleteUsuarioClienteAsync(int id);


    }
}
