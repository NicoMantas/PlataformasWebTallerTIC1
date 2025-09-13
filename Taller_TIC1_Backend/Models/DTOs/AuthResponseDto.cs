
namespace Taller_TIC1_Backend.Models.DTOs
{
    public class AuthResponseDto
    {
        // DTO para la respuesta de autenticación
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public UserInfoDto? User { get; set; }

    }
}
