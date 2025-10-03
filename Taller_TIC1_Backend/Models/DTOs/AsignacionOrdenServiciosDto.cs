namespace Taller_TIC1_Backend.Models.DTOs
{
    public class AsignacionOrdenServiciosDto
    {
        public int OrdenId { get; set; }
        public List<int> ServiciosIds { get; set; } = new();
    }
}
