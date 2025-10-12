namespace Taller_TIC1_Backend.Models
{
    public class Repuesto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public long Numero_serie { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
    }
}
