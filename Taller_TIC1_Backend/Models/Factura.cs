namespace Taller_TIC1_Backend.Models
{
    public class Factura
    {
        public int Id { get; set; }
        public int IdOrdenTrabajo { get; set; }
        public DateTime FechaEmision { get; set; }
        public float Subtotal { get; set; }
        public float Impuestos { get; set; }
        public float Total { get; set; }
        public string Estado { get; set; } = string.Empty; // "Pendiente", "Pagada", "Cancelada"
        
        // Navegación
        public OrdenDeTrabajo? OrdenTrabajo { get; set; }
    }
}
