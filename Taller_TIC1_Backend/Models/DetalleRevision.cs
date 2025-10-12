namespace Taller_TIC1_Backend.Models
{
    public class DetalleRevision
    {
        public int IdServicio { get; set; }
        public string Detalles { get; set; } = string.Empty;
        public string? DetallesEncontrados { get; set; }
    }
}
