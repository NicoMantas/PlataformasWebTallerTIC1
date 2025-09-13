using System.ComponentModel.DataAnnotations;

namespace Taller_TIC1_Backend.Models.DTOs
{
    public class EmpleadoUpdateDto
    {
        [Required]
        [MaxLength(255)]
        public string Nombre { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(255)]
        public string Apellido { get; set; } = string.Empty;
        
        [Required]
        public long Cedula { get; set; }
        
        [Required]
        [Range(0, double.MaxValue)]
        public double Salario { get; set; }
        
        [Required]
        public int IdTipoEmpleado { get; set; }
    }
}
