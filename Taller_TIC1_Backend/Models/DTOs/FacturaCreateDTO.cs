namespace Taller_TIC1_Backend.Models.DTOs
{
    public class FacturaCreateDTO
    {
        public int IdOrdenTrabajo { get; set; }
        public DateTime FechaEmision { get; set; } = DateTime.Now;
        public float Subtotal { get; set; }
        public float Impuestos { get; set; }
        public float Total { get; set; }
        public string Estado { get; set; } = "Pendiente";
    }
}
