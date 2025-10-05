namespace Taller_TIC1_Backend.Models.DTOs
{
    public class VehiculoResponseDto
    {
        public int Id { get; set; }
        public string Placa { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public int Anio { get; set; }
        public string TipoVehiculo { get; set; } = string.Empty;
        public int? Cilindraje { get; set; }
        public int? CapacidadBateria { get; set; }
        public int IdCliente { get; set; }

    }
}
