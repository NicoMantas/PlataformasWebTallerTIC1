using B_TallerAutomoviles.Clases;
using System.Text.Json.Serialization;
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
        [JsonIgnore]
        public Vehiculo? Vehiculo { get; set; }
        //propiedades para determinar tipo de cliente
        [JsonIgnore]
        public CNatural? CNatural { get; set; }
        [JsonIgnore]
        public CEmpresa? CEmpresa { get; set; }

        // Propiedad para determinar el tipo
        [JsonIgnore]
        public string TipoCliente
        {
            get
            {
                if (CNatural != null) return "Natural";
                if (CEmpresa != null) return "Empresa";
                return "General";
            }
        }
    }
}
