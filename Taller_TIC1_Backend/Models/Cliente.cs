using B_TallerAutomoviles.Clases;

namespace Taller_TIC1_Backend.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public long Telefono { get; set; }
        public int IdVehiculo { get; set; }
        
        // Propiedades de navegación
        public Vehiculo? Vehiculo { get; set; }
    }
}
