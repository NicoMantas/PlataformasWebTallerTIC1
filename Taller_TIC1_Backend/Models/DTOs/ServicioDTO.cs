namespace Taller_TIC1_Backend.Models.DTOs
{
    public class ServicioDTO
    {
        public int Id { get; set; }
        public decimal Costo { get; set; }
        public int IdCliente { get; set; }
        public int IdVehiculo { get; set; }
        public int? IdEmpleado { get; set; }
        public int IdEstado { get; set; }
        public string? TipoServicio { get; set; } // "Revision" o "Reparacion"
        public string? DetallesRevision { get; set; }
        public string? DetallesEncontrados { get; set; }
        public List<RepuestoCantidadDTO>? RepuestosReparacion { get; set; }
        public string? EstadoDescripcion { get; set; }
        public string? ClienteNombre { get; set; }
        public string? EmpleadoNombre { get; set; }
        public string? VehiculoPlaca { get; set; }
        public string? VehiculoMarca { get; set; }
        public string? VehiculoModelo { get; set; }
        public string? VehiculoInfo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}
