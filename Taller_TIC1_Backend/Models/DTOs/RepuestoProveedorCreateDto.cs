using System.ComponentModel.DataAnnotations;

namespace Taller_TIC1_Backend.Models.DTOs
{
    public class RepuestoProveedorCreateDto
    {
        [Required]
        public int IdRepuesto { get; set; }

        [Required]
        public int IdProveedor { get; set; }
    }
}
