using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller_backend.Models
{
    [Table("Mecanico")]
    public class Mecanico
    {
        [Key]
        [Column("idEmpleado")]
        public long IdEmpleado { get; set; }

        [Column("especialidad")]
        [MaxLength(100)]
        public string? Especialidad { get; set; }

        [Column("tareasTrabajadas")]
        [MaxLength(500)]
        public string? TareasTrabajadas { get; set; }

        // Propiedades de navegación
        [ForeignKey("IdEmpleado")]
        public Empleado? Empleado { get; set; }
    }
}
