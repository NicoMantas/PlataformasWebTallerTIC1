namespace Taller_TIC1_Backend.Models.DTOs
{
    public class EmpleadoValidacionFechasDto
    {
        public DateTime? FechaDesactivacion { get; set; }
        public DateTime? FechaActivacion { get; set; }
        public DateTime FechaContratacion { get; set; }
    }
}
