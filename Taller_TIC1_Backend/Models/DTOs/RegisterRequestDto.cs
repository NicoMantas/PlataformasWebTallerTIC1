using System.ComponentModel.DataAnnotations;

namespace Taller_TIC1_Backend.Models.DTOs
{
    public class RegisterRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public int IdTaller { get; set; }

        // Para empleados
        public int? IdEmpleado { get; set; }

        // Para clientes
        public int? IdCliente { get; set; }
    }
}
