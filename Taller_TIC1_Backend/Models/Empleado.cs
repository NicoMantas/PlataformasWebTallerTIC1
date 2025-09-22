using B_TallerAutomoviles.Clases;
using System.Text.Json.Serialization;

namespace Taller_TIC1_Backend.Models
{
    public class Empleado
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public long Cedula { get; set; }
        public double Salario { get; set; }
        public DateTime FechaContratacion { get; set; }
        public int IdTipoEmpleado { get; set; }

        // Propiedad de navegación
        [JsonIgnore]
        public TipoEmpleado? TipoEmpleado { get; set; }
    }
}
