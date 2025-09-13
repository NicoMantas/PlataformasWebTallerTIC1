using B_TallerAutomoviles.Clases;

namespace Taller_TIC1_Backend.Models
{
    public class OrdenDeTrabajo
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdTipoEstadoOrden { get; set; }
        
        // Propiedad de navegación
        public TipoEstadoOrden? TipoEstadoOrden { get; set; }
    }
}
