namespace taller_backend.DTOs
{
    public record DetalleTipoPersonaCreateDto(string? Descripcion, int? IdTipoPersona);
    public record DetalleTipoPersonaUpdateDto(string? Descripcion, int? IdTipoPersona);
}