namespace Taller_TIC1_Backend.Models.DTOs
{
    public class TallerResponseDto
    {
        // DTO para devolver la información de un taller
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
    }
}
