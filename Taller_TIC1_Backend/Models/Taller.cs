namespace Taller_TIC1_Backend.Models
{
    public class Taller
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;

        // Propiedades de navegación
        public List<UsuarioEmpleadoTaller> UsuariosEmpleados { get; set; } = new List<UsuarioEmpleadoTaller>();
        public List<UsuarioClienteTaller> UsuariosClientes { get; set; } = new List<UsuarioClienteTaller>();
    }
}
