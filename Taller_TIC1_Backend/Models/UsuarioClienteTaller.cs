namespace Taller_TIC1_Backend.Models
{
    public class UsuarioClienteTaller
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public int IdTaller { get; set; }
        public int IdCliente { get; set; }

        // Propiedades de navegación CORRECTAS
        public Taller? Taller { get; set; }
        public Cliente? Cliente { get; set; }
    }
}
