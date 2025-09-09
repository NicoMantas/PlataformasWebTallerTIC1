using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; //para mapear la clase con la tabla de la base de datos

namespace taller_backend.Models
{
    [Table("Persona")]
    public class Persona
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("nombre")]
        [MaxLength(100)]
        public string? Nombre { get; set; }

        [Column("telefono")]
        public long? Telefono { get; set; }

        [Column("Email")]
        public string? Email { get; set; }

        [Column("idDetalleTipoPersona")]
        public long? idDetalleTipoPersona { get; set; }

        //fecha de creacion
        [Column("fechaCreacion")]
        public DateTime? fechaCreacion { get; set; }


        //propiedades de navegacion.
        [ForeignKey("IdTDetalleTipoPersona")]
        public DetalleTipoPersona? DetalleTipoPersona { get; set; }

        //coleccion de navegacion
        public Usuario? Usuario { get; set; }
        public ClienteNatural? ClienteNatural { get; set; }
        public ClienteJuridico? ClienteJuridico { get; set; }
    }

}

