using System.ComponentModel.DataAnnotations;
namespace Taller_TIC1_Backend.Models.DTOs
{
    public class RepuestoResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public long? NumeroSerie { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public List<ProveedorResponseDto> Proveedores { get; set; } = new();
    }
}
