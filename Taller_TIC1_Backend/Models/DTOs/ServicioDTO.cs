namespace Taller_TIC1_Backend.Models.DTOs
{
    public class ServicioDTO
    {
        public int Id { get; set; }
        public float Costo { get; set; }
        public int IdCliente { get; set; }
        public int? IdEmpleado { get; set; }
        public int IdEstado { get; set; }
        public string? TipoServicio { get; set; } // "Revision" o "Reparacion"
        public string? DetallesRevision { get; set; }
        public List<RepuestoCantidadDTO>? RepuestosReparacion { get; set; }
        public string? EstadoDescripcion { get; set; }
        public string? ClienteNombre { get; set; }
        public string? EmpleadoNombre { get; set; }
    }
}
