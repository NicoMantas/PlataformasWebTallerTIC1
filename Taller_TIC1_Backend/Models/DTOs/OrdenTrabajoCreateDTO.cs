namespace Taller_TIC1_Backend.Models.DTOs
{
    public class OrdenTrabajoCreateDTO
    {
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public int IdTipoEstadoOrden { get; set; } = 1; // Pendiente por defecto
        public int IdCliente { get; set; }
        public int IdVehiculo { get; set; }
        public List<int> ServiciosIds { get; set; } = new List<int>();
        public string? Descripcion { get; set; }
    }
}
