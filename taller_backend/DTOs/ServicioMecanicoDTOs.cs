namespace taller_backend.DTOs
{
    //solo agregar mecanico a un servicio
    public record ServicioMecanicoCreateDto(long IdServicio, long IdMecanico);
}