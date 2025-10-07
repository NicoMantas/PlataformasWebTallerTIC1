namespace Taller_TIC1_Backend.Models.DTOs
{
    public class ServicioCreateDTO
    {
        public float Costo { get; set; }
        public int IdCliente { get; set; }
        public int? IdEmpleado { get; set; }
        public int IdEstado { get; set; } = 1; // Pendiente por defecto
        public string TipoServicio { get; set; } = string.Empty; // "Revision" o "Reparacion"
        public string? DetallesRevision { get; set; }
        public List<RepuestoCantidadDTO>? RepuestosReparacion { get; set; }
    }
}
