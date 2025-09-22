namespace Taller_TIC1_Backend.Models.DTOs
{
    public class OrdenTrabajoCreateDTO
    {
        public DateTime FechaCreacion { get; set; }
        public int IdTipoEstadoOrden { get; set; }
        public List<int> ServiciosIds { get; set; } = new List<int>();
    }
}
