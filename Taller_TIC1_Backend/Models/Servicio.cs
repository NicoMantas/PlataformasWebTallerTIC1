using B_TallerAutomoviles.Clases;

namespace Taller_TIC1_Backend.Models
{
    public class Servicio
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public int? IdEmpleado { get; set; }
        public int IdEstado { get; set; }
        public float Costo { get; set; }
        
        // Propiedades de navegación
        public Cliente? Cliente { get; set; }
        public Empleado? Empleado { get; set; }
        public EstadoServicio? Estado { get; set; }
    }
}
