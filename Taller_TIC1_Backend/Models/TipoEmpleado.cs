using System.Text.Json.Serialization;
namespace Taller_TIC1_Backend.Models
{
    public class TipoEmpleado
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;


        // Propiedad de navegación
        [JsonIgnore]
        public List<Empleado> Empleados { get; set; } = new List<Empleado>();
    }
}
