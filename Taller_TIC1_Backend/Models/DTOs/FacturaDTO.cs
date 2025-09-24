namespace Taller_TIC1_Backend.Models.DTOs
{
    public class FacturaDTO
    {
        public int Id { get; set; }
        public int IdOrdenTrabajo { get; set; }
        public DateTime FechaEmision { get; set; }
        public float Subtotal { get; set; }
        public float Impuestos { get; set; }
        public float Total { get; set; }
        public string Estado { get; set; } = string.Empty; // "Pendiente", "Pagada", "Cancelada"
        public string? ClienteNombre { get; set; }
        public string? OrdenDescripcion { get; set; }
    }
}
