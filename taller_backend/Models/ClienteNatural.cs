using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller_backend.Models
{
    [Table("ClienteNatural")]
    public class ClienteNatural
    {
        [Key]
        [Column("idPersona")]
        public long IdPersona { get; set; }

        [Column("cedula")]
        [Required]
        public long Cedula { get; set; }

        [Column("apellido")]
        public string? Apellido { get; set; }

        // Propiedades de navegación
        [ForeignKey("IdPersona")]
        public Persona? Persona { get; set; }
    }
}
