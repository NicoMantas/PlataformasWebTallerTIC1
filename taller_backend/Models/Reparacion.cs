using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller_backend.Models
{
    [Table("Reparacion")]
    public class Reparacion
    {
        [Key]
        [Column("idServicio")]
        public long IdServicio { get; set; }

        [Column("mano_de_obra")]
        public double? ManoDeObra { get; set; }

        public Servicio? Servicio { get; set; }
    }
}
