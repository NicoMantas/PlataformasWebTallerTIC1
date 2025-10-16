using System.ComponentModel.DataAnnotations;

namespace Taller_TIC1_Backend.Models.DTOs
{
    public class EmpleadoDesactivarDto
    {
        [Required(ErrorMessage = "La razón de desactivación es requerida")]
        [StringLength(500, ErrorMessage = "La razón no puede exceder los 500 caracteres")]
        public string DetallesDesactivacion { get; set; } = string.Empty;
        
        public DateTime? FechaDesactivacion { get; set; }
        public DateTime? FechaActivacion { get; set; }
    }

    public class EmpleadoActivarDto
    {
        public DateTime? FechaActivacion { get; set; }
    }
}
