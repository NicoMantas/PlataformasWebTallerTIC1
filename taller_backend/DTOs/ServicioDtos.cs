namespace taller_backend.DTOs
{
    public record ServicioCreateDto(
        string? Descripcion,
        double? CostoBase,
        int? TiempoEstimado,
        long? IdEstado,
        long? IdTipoServicio
    );

    public record ServicioUpdateDto(
        string? Descripcion,
        double? CostoBase,
        int? TiempoEstimado,
        long? IdEstado,
        long? IdTipoServicio
    );

    public record ReparacionCreateDto(double? ManoDeObra);
    public record RevisionCreateDto(string? Diagnostico);
}