namespace Taller_TIC1_Backend.Models.DTOs
{
    public class DetalleRevisionDTO
    {
        public int IdServicio { get; set; }
        public string Detalles { get; set; } = string.Empty;
        public string? DetallesEncontrados { get; set; }
    }

    public class DetalleRevisionCreateDTO
    {
        public int IdServicio { get; set; }
        public string Detalles { get; set; } = string.Empty;
        public string? DetallesEncontrados { get; set; }
    }

    public class DetalleRevisionUpdateDTO
    {
        public int IdServicio { get; set; }
        public string Detalles { get; set; } = string.Empty;
        public string? DetallesEncontrados { get; set; }
    }
}
