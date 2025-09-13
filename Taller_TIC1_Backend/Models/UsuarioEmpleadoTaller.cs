namespace Taller_TIC1_Backend.Models
{
    public class UsuarioEmpleadoTaller
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public int IdTaller { get; set; }
        public int IdEmpleado { get; set; }
    }
}
