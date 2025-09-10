using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; //para mapear la clase con la tabla de la base de datos

namespace taller_backend.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("idPersona")]
        public long IdPersona { get; set; }

        [Column("username")]
        [MaxLength(50)]
        [Required]
        public string Username { get; set; } = string.Empty;

        [Column("passwordHash")]
        [MaxLength(255)]
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Column("activo")]
        public bool Activo { get; set; } = true;

        [Column("fechaCreacion")]
        public DateTime? FechaCreacion { get; set; }

        [Column("ultimoLogin")]
        public DateTime? UltimoLogin { get; set; }

        // Propiedades de navegación
        [ForeignKey("IdPersona")]
        public Persona? Persona { get; set; }
    }
}
