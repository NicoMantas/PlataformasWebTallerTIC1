using System.ComponentModel.DataAnnotations;

namespace Taller_TIC1_Backend.Models.DTOs
{
    public class VehiculoUpdateDto
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
        public int Anio { get; set; }

        public int? IdCliente { get; set; }

        // Campos para subtipos (opcionales)
        public string? Tipo { get; set; } // "Gasolina", "Electrico", "Hibrido"
        public int? Cilindraje { get; set; }
        public int? CapacidadBateria { get; set; }
    }
}