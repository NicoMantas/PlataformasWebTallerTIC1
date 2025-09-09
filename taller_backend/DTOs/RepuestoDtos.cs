namespace taller_backend.DTOs
{
    //este record es para crear un nuevo repuesto
    public record RepuestoCreateDto(
        string? Nombre,
        long? NumeroParte,
        string? Descripcion,
        double? CostoCompra,
        double? PrecioVenta,
        int? CantidadStock,
        long? IdProveedor
    );

    //este record es para actualizar un repuesto
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
