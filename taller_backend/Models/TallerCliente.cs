using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller_backend.Models
{
    [Table("TallerCliente")]
    public class TallerCliente
    {
        [Column("idTaller")]
        public long IdTaller { get; set; }

        [Column("idCliente")]
        public long IdCliente { get; set; }

        [Column("fechaRegistro")]
        public DateTime? FechaRegistro { get; set; }

        // Propiedades de navegación
        [ForeignKey("IdTaller")]
        public Taller? Taller { get; set; }

        [ForeignKey("IdCliente")]
        public Persona? Cliente { get; set; }
    }
}
