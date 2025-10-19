namespace Taller_TIC1_Backend.Models.DTOs
{
    public class FacturaCreateDTO
    {
        public int IdOrdenTrabajo { get; set; }
        public DateTime FechaEmision { get; set; } = DateTime.Now;
        public decimal Subtotal { get; set; }
        public decimal Impuestos { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; } = "Pendiente";
    }
}
