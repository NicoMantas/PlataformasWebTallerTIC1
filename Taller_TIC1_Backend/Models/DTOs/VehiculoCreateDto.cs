using System.ComponentModel.DataAnnotations;

namespace Taller_TIC1_Backend.Models.DTOs
{
    public class VehiculoCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string Placa { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string Marca { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string Modelo { get; set; } = string.Empty;
        
        [Required]
        [Range(1900, 2030)]
        public int Año { get; set; }
    }
}
