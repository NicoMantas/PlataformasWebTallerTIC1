namespace Taller_TIC1_Backend.Models
{
    public class VElectrico
    {
        public int IdVehiculo { get; set; }
        public int CapacidadBateria { get; set; }
        
        // Propiedad de navegación
        public Vehiculo? Vehiculo { get; set; }
    }
}
