using System.ComponentModel.DataAnnotations.Schema;

namespace taller_backend.Models
{
    [Table("ServicioMecanicos")]
    public class ServicioMecanico
    {
        [Column("idServicio")]
        public long IdServicio { get; set; }

        [Column("idMecanico")]
        public long IdMecanico { get; set; }

        [Column("fechaAsignacion")]
        public DateTime? FechaAsignacion { get; set; }

        [ForeignKey("IdServicio")]
        public Servicio? Servicio { get; set; }

        [ForeignKey("IdMecanico")]
        public Mecanico? Mecanico { get; set; }
    }
}