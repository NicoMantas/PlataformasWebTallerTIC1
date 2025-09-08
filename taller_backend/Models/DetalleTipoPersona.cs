using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; //para mapear la clase con la tabla de la base de datos

namespace taller_backend.Models
{
    public class DetalleTipoPersona
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("descripcion")]
        public string? Descripcion { get; set; }

        // navegación (colección)
        public ICollection<Persona> Personas { get; set; } = new List<Persona>();

    }
}
