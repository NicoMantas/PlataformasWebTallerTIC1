using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller_backend.Models
{
    [Table("TipoServicio")]
    public class TipoServicio
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("descripcion")]
        public string? Descripcion { get; set; }
    }
}