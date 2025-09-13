namespace Taller_TIC1_Backend.Models.DTOs
{
    public class RepuestoProveedorResponseDto
    {
        public int IdRepuesto { get; set; }
        public int IdProveedor { get; set; }
        public RepuestoResponseDto? Repuesto { get; set; }
        public ProveedorResponseDto? Proveedor { get; set; }
    }
}
