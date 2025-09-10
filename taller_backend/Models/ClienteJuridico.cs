using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; //para mapear la clase con la tabla de la base de datos


namespace taller_backend.Models
{
    [Table("ClienteJuridico")]
    public class ClienteJuridico
    {
        //establecer reglas de validacion de datos
        [Key]
        [Column("idPersona")]
        public long IdPersona { get; set; }

        [Column("nit")]
        [Required]  // El NIT es obligatorio
        public long Nit { get; set; }

        [Column("representanteLegal")]
        [MaxLength(100)] // Máximo 100 caracteres para el representante legal
        public string? RepresentanteLegal { get; set; }

        // Propiedades de navegación
        [ForeignKey("IdPersona")]
        public Persona? Persona { get; set; }
    }
}
