namespace Taller_TIC1_Backend.Models.DTOs
{
    public class FacturaDTO
    {
        public int Id { get; set; }
        public int IdOrdenTrabajo { get; set; }
        public DateTime FechaEmision { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Impuestos { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; } = string.Empty; // "Pendiente", "Pagada", "Cancelada"

        //propiedad de navegaci�n para acceder al nombre del cliente desde la orden de trabajo
        public string? ClienteNombre { get; set; }

        //propiedad de navegaci�n para acceder a la placa del veh�culo desde la orden de trabajo
        public string? VehiculoPlaca { get; set; }

        // Lista de descripciones de servicios (Revision/Reparacion) realizados en la orden de trabajo 
        public List<string>? ServiciosRealizados { get; set; } 


    }
}
