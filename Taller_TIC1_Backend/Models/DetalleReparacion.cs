using System.Text.Json.Serialization;

namespace Taller_TIC1_Backend.Models
{
    public class DetalleReparacion
    {
        public int IdServicio { get; set; }
        public int IdDetalleReparacionRepuesto { get; set; }

        // Propiedades de navegaci�n
        public Servicio? Servicio { get; set; }
        public DetalleReparacionRepuesto? DetalleReparacionRepuesto { get; set; }
    }
}
