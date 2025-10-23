namespace Taller_TIC1_Backend.Models
{
    public class Factura
    {
        public int Id { get; set; }
        public int IdOrdenTrabajo { get; set; }
        public DateTime FechaEmision { get; set; }
        // public DateTime? FechaActualizacion { get; set; } // Temporalmente comentado hasta crear migración
        public decimal Subtotal { get; set; }
        public decimal Impuestos { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; } = string.Empty; // "Pendiente", "Pagada", "Cancelada"
        
        // Navegación
        public OrdenDeTrabajo? OrdenTrabajo { get; set; }
    }
}
