using B_TallerAutomoviles.Clases;

namespace Taller_TIC1_Backend.Models
{
    public class Vehiculo
    {
        public int Id { get; set; }
        public string Placa { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public int Anio { get; set; }
        public int? IdCliente { get; set; }

        // Propiedad para determinar el tipo
        public string TipoVehiculo
        {
            get
            {
                if (VGasolina != null) return "Gasolina";
                if (VElectrico != null) return "Electrico";
                if (VHibrido != null) return "Hibrido";
                return "General";
            }
        }

        // Propiedades de navegación para los subtipos
        public VGasolina? VGasolina { get; set; }
        public VElectrico? VElectrico { get; set; }
        public VHibrido? VHibrido { get; set; }
    }
}
