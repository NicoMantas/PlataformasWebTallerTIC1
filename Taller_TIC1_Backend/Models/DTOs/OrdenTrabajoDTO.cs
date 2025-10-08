namespace Taller_TIC1_Backend.Models.DTOs
{
    public class OrdenTrabajoDTO
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdTipoEstadoOrden { get; set; }
        public int IdCliente { get; set; }
        public int IdVehiculo { get; set; }
        public List<int> ServiciosIds { get; set; } = new List<int>();
        public string? EstadoDescripcion { get; set; }
        public string? Descripcion { get; set; }
        public string? ClienteNombre { get; set; }
        public string? VehiculoPlaca { get; set; }
    }
}
