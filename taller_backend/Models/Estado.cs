using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller_backend.Models
{
    [Table("Estado")]
    public class Estado
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("descripcion")]
        public string? Descripcion { get; set; }
    }
}
