namespace Taller_TIC1_Backend.Models.DTOs
{
    public class EmpleadoResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public long Cedula { get; set; }
        public double Salario { get; set; }
        public DateTime FechaContratacion { get; set; }
        public int IdTipoEmpleado { get; set; }
        public bool Activo { get; set; } = true;
        public string? DetallesDesactivacion { get; set; }
        public DateTime? FechaDesactivacion { get; set; }
        public string? TipoEmpleadoDescripcion { get; set; }
    }
}
