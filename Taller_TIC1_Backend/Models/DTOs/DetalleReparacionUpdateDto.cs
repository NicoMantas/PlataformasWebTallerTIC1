using System.ComponentModel.DataAnnotations;

namespace Taller_TIC1_Backend.Models.DTOs
{
    public class DetalleReparacionUpdateDto
    {
        [Required] public int IdServicio { get; set; }
        [Required] public int IdDetalleReparacionRepuesto { get; set; }
    }
}
