namespace taller_backend.DTOs
{
    public record DetalleTipoPersonaCreateDto(string? Descripcion, long? IdTipoPersona);
    public record DetalleTipoPersonaUpdateDto(string? Descripcion, long? IdTipoPersona);

}