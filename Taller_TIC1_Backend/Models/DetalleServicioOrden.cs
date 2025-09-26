namespace Taller_TIC1_Backend.Models
{
    public class DetalleServicioOrden
    {
        public int IdOrdenTrabajo { get; set; }
        public int IdServicio { get; set; }
        
        // Propiedades de navegación
        public OrdenDeTrabajo? OrdenTrabajo { get; set; }
        public Servicio? Servicio { get; set; }
    }
}
