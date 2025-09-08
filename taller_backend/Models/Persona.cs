using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; //para mapear la clase con la tabla de la base de datos

namespace taller_backend.Models
{
    public class Persona
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("nombre")]
        public string? Nombre { get; set; }

        [Column("telefono")]
        public string? Telefono { get; set; }

        [Column("idDetalleTipoPersona")]
        public long? idDetalleTipoPersona { get; set; }

        //fecha de creacion
        [Column("fechaCreacion")]
        public DateTime? fechaCreacion { get; set; }


    }
}
