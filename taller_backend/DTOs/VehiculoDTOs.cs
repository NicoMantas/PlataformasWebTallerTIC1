namespace taller_backend.DTOs
{
    public record VehiculoCreateDto(
        string Placa,
        string? Marca,
        string? Modelo,
        int? Año,
        long? IdPersona,
        long? IdTipoVehiculo
    );

    public record VehiculoUpdateDto(
        string? Marca,
        string? Modelo,
        int? Año,
        long? IdPersona,
        long? IdTipoVehiculo
    );
}