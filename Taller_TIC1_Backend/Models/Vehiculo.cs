using B_TallerAutomoviles.Clases;

namespace Taller_TIC1_Backend.Models
{
    public class Vehiculo
    {
        public int Id { get; set; }
        public string Placa { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public int Año { get; set; }
    }
}
