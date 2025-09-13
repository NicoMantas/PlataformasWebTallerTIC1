using System.ComponentModel.DataAnnotations;

namespace Taller_TIC1_Backend.Models.DTOs
{
    public class PorveedorCreateDTOcs
    {
        public class ProveedorCreateDto
        {
            [Required]
            [StringLength(100)]
            public string Nombre { get; set; } = string.Empty;

            [StringLength(100)]
            public string? Contacto { get; set; }

        }
    }
}
