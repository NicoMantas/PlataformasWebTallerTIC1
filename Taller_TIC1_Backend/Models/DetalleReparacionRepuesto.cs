namespace Taller_TIC1_Backend.Models
{
    public class DetalleReparacionRepuesto
    {
        public int Id { get; set; }
        public int IdRepuesto { get; set; }
        public int Cantidad { get; set; }

        // Propiedades de navegación
        public Repuesto Repuesto { get; set; }

    }
}
