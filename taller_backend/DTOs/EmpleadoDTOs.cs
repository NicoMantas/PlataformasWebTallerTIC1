using System;

namespace taller_backend.DTOs
{
    public record EmpleadoCreateDto(string? Nombre, string? Apellido, long? Cedula, double? Salario, DateTime? FechaContratacion, int? IdDetalleTipoPersona);
    public record EmpleadoUpdateDto(string? Nombre, string? Apellido, long? Cedula, double? Salario, DateTime? FechaContratacion, int? IdDetalleTipoPersona);
}
