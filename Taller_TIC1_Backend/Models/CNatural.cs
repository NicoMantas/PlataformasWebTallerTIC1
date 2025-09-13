namespace Taller_TIC1_Backend.Models
{
    public class CNatural
    {
        public int IdCliente { get; set; }
        public long Cedula { get; set; }
        public string Apellido { get; set; } = string.Empty;
        
        // Propiedad de navegación
        public Cliente? Cliente { get; set; }
    }
}
