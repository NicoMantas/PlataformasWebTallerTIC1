using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller_backend.Models
{
    [Table("OrdenServicio")]
    public class OrdenServicio
    {
        [Column("idOrden")]
        public long IdOrden { get; set; }

        [Column("idServicio")]
        public long IdServicio { get; set; }

        // Propiedades de navegación
        [ForeignKey("IdOrden")]
        public OrdenDeTrabajo? OrdenDeTrabajo { get; set; }

        [ForeignKey("IdServicio")]
        public Servicio? Servicio { get; set; }
    }
}
