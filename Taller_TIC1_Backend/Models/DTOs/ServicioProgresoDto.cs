namespace Taller_TIC1_Backend.Models.DTOs
{
    public class ServicioProgresoDto
    {
        public int Id { get; set; }
        public string EstadoActual { get; set; } = string.Empty;
        public int IdEstado { get; set; }
        public string TipoServicio { get; set; } = string.Empty;
        public string ClienteNombre { get; set; } = string.Empty;
        public string VehiculoPlaca { get; set; } = string.Empty;
        public string VehiculoMarca { get; set; } = string.Empty;
        public string VehiculoModelo { get; set; } = string.Empty;
        public string? MecanicoNombre { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public decimal Costo { get; set; }
        public List<EstadoProgresoDto> FlujoEstados { get; set; } = new List<EstadoProgresoDto>();
        public int PorcentajeCompletado { get; set; }
        public string MensajeEstado { get; set; } = string.Empty;
    }

    public class EstadoProgresoDto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public bool Completado { get; set; }
        public bool Actual { get; set; }
        public DateTime? FechaCompletado { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public string Icono { get; set; } = string.Empty;
    }
}
