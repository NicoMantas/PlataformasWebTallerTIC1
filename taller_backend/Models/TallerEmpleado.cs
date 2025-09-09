using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller_backend.Models
{
    [Table("TallerEmpleado")]
    public class TallerEmpleado
    {
        [Key]
        [Column("idTaller")]
        public long IdTaller { get; set; }

        [Key]
        [Column("idEmpleado")]
        public long IdEmpleado { get; set; }

        [Column("fechaContratacion")]
        public DateTime? FechaContratacion { get; set; }

        // Propiedades de navegación
        [ForeignKey("IdTaller")]
        public Taller? Taller { get; set; }

        [ForeignKey("IdEmpleado")]
        public Empleado? Empleado { get; set; }
    }
}
