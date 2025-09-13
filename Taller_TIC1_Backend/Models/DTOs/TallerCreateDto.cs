using System.ComponentModel.DataAnnotations;

namespace Taller_TIC1_Backend.Models.DTOs
{
    public class TallerCreateDto
    {
        [Required]
        [MaxLength(255)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Direccion { get; set; } = string.Empty;
    }
}
