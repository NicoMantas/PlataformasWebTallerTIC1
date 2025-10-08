namespace Taller_TIC1_Backend.Models.DTOs
{
    public class UpdateUsuarioEmpleadoDto
    {
            public string Email { get; set; } = string.Empty;
            public string? Password { get; set; }
            public int IdTaller { get; set; }
            public int IdEmpleado { get; set; }
        
    }
}

