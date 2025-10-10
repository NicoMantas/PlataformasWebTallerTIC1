using B_TallerAutomoviles.Clases;

namespace Taller_TIC1_Backend.Models
{
    public class Servicio
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public int IdVehiculo { get; set; }
        public int? IdEmpleado { get; set; }
        public int IdEstado { get; set; }
        public float Costo { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaActualizacion { get; set; }
        
        // Propiedades de navegación
        public Cliente? Cliente { get; set; }
        public Vehiculo? Vehiculo { get; set; }
        public Empleado? Empleado { get; set; }
        public EstadoServicio? Estado { get; set; }
    }
}
