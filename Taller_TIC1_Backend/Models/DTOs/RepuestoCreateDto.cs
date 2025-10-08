using System.ComponentModel.DataAnnotations;
namespace Taller_TIC1_Backend.Models.DTOs
{
    public class RepuestoCreateDto
    {
        [Required]
        [StringLength(255)]
        public string Nombre { get; set; } = string.Empty;

        public long? NumeroSerie { get; set; }

        [Required]
        public float Precio { get; set; }

        [Required]
        public int Stock { get; set; }
    }
}
