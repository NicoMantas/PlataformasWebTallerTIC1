namespace taller_backend.DTOs
{
    public record PersonaCreateDto(string? Nombre, string? Email, long? Telefono, long? IdTDetalleTipoPersona);
    public record PersonaUpdateDto(string? Nombre, string? Email, long? Telefono, long? IdTDetalleTipoPersona);
}
