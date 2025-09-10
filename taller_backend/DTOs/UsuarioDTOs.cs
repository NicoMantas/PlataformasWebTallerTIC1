namespace taller_backend.DTOs
{
    public record UsuariosCreateDto(long? IdPersona, string? Username, string? PasswordHash, bool? Activo);
    public record UsuariosUpdateDto(long? IdPersona, string? Username, string? PasswordHash, bool? Activo);
}