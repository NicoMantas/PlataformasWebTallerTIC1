using System.Text.Json.Serialization;
namespace Taller_TIC1_Backend.Models.DTOs
{
    public class ClienteResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public long Telefono { get; set; }
        public int IdVehiculo { get; set; }
        public string TipoCliente { get; set; } = string.Empty;
        public long? Cedula { get; set; }
        public string? Apellido { get; set; }
        public long? Nit { get; set; }
        public string? RepresentanteLegal { get; set; }
        public string? VehiculoInfo { get; set; }

    }
}
