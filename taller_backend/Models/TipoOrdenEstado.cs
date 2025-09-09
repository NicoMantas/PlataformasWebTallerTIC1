using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller_backend.Models
{
    [Table("TipoOrdenEstado")]
    public class TipoOrdenEstado
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("descripcion")]
        [MaxLength(50)]
        public string? Descripcion { get; set; }

        // Colección de navegación
        public ICollection<OrdenDeTrabajo> OrdenesTrabajo { get; set; } = new List<OrdenDeTrabajo>();
    }
}
