namespace taller_backend.DTOs
{
    public record ClienteNaturalCreateDto(long IdPersona, long? Cedula, string? Apellido);
    public record ClienteNaturalUpdateDto(long? Cedula, string? Apellido);

    public record ClienteJuridicoCreateDto(long IdPersona, long? Nit, string? RepresentanteLegal);
    public record ClienteJuridicoUpdateDto(long? Nit, string? RepresentanteLegal);
}