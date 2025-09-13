using System.ComponentModel.DataAnnotations;

namespace Taller_TIC1_Backend.Models.DTOs
{
    public class ClienteCreateDto
    {
        [Required]
        [MaxLength(255)]
        public string Nombre { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public long Telefono { get; set; }
        
        [Required]
        public int IdVehiculo { get; set; }

        // Campos para subtipos
        public string? Tipo { get; set; } // "Natural" o "Empresa"
        public long? Cedula { get; set; }
        public string? Apellido { get; set; }
        public long? Nit { get; set; }
        public string? RepresentanteLegal { get; set; }
    }
}
