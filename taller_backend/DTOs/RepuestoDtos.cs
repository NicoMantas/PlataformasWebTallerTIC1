namespace taller_backend.DTOs
{
    public record RepuestoCreateDto(
    long Id,
    string? Nombre,
    long? NumeroParte,
    string? Descripcion,
    double? CostoCompra,
    double? PrecioVenta,
    int? CantidadStock,
    long? IdProveedor
);

    public record RepuestoUpdateDto(
        string? Nombre,
        long? NumeroParte,
        string? Descripcion,
        double? CostoCompra,
        double? PrecioVenta,
        int? CantidadStock,
        long? IdProveedor
    );
}
