using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace taller_backend.Models
{
    [Table("Servicio")]
    public class Servicio
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("descripcion")]
        public string? Descripcion { get; set; }

        [Column("costoBase")]
        public double? CostoBase { get; set; }

        [Column("tiempoEstimado")]
        public int? TiempoEstimado { get; set; }

        [Column("idEstado")]
        public long? IdEstado { get; set; }

        [Column("idTipoServicio")]
        public long? IdTipoServicio { get; set; }

        public Estado? Estado { get; set; } 
        public TipoServicio? TipoServicio { get; set; } //navegación muchos a uno
        public Reparacion? Reparacion { get; set; } //navegación uno a uno
        public Revision? Revision { get; set; } //navegación uno a uno
    }
}
}
