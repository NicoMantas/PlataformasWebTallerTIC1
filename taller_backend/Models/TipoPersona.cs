using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; //para mapear la clase con la tabla de la base de datos

namespace taller_backend.Models
{
    [Table("TipoPersona")]
    public class TipoPersona
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("descripcion")]
        [MaxLength(50)]
        public string? Descripcion { get; set; }

        // Colección de navegación
        public ICollection<DetalleTipoPersona> DetallesTipoPersona { get; set; } = new List<DetalleTipoPersona>();
    }
}
