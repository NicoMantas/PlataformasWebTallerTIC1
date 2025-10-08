namespace Taller_TIC1_Backend.Models
{
    public class VGasolina
    {
        public int IdVehiculo { get; set; }
        public int Cilindraje { get; set; }
        
        // Propiedad de navegación
        public Vehiculo? Vehiculo { get; set; }
    }
}
