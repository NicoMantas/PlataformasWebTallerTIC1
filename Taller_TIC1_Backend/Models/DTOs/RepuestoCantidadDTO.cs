namespace Taller_TIC1_Backend.Models.DTOs
{
    public class RepuestoCantidadDTO
    {
        public int IdRepuesto { get; set; }
        public string NombreRepuesto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public float PrecioUnitario { get; set; }
        public float Subtotal { get; set; }
    }
}