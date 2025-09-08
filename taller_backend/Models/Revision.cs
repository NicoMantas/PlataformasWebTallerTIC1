using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller_backend.Models
{
    [Table("Revision")]
    public class Revision
    {
        [Key]
        [Column("idServicio")]
        public long IdServicio { get; set; }

        [Column("diagnostico")]
        public string? Diagnostico { get; set; }

        public Servicio? Servicio { get; set; }
    }
}