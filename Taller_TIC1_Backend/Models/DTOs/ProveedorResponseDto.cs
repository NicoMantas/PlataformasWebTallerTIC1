namespace Taller_TIC1_Backend.Models.DTOs
{
    public class ProveedorResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Contacto { get; set; }
        public List<RepuestoResponseDto> Repuestos { get; set; } = new();
    }
}
