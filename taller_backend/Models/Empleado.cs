using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller_backend.Models
{
    public class Empleado
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("nombre")]
        public string? Nombre { get; set; }

        [Column("apellido")]
        public string? Apellido { get; set; }

        [Column("cedula")]
        [Required] // La cédula es obligatoria
        public long Cedula { get; set; }

        [Column("salario")]
        public double? Salario { get; set; }

        [Column("fechaContratacion")]
        public DateTime? FechaContratacion { get; set; }

        [Column("idDetalleTipoPersona")]
        public int IdDetalleTipoPersona { get; set; }

        // Propiedades de navegación
        [ForeignKey("IdDetalleTipoPersona")]
        public DetalleTipoPersona? DetalleTipoPersona { get; set; }
    }
}
