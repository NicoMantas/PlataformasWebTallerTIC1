namespace Taller_TIC1_Backend.Models.DTOs
{
    public class UserInfoDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string TipoUsuario { get; set; } = string.Empty; // "empleado" o "cliente"
        public int IdTaller { get; set; }
        public string NombreTaller { get; set; } = string.Empty;
        public object? InfoEspecifica { get; set; }
    }
}
