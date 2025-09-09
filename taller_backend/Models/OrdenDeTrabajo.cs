using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller_backend.Models
{
    [Table("OrdenDeTrabajo")]
    public class OrdenDeTrabajo
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("fechaCreacion")]
        public DateTime? FechaCreacion { get; set; }

        [Column("idTipoOrdenEstado")]
        public long IdTipoOrdenEstado { get; set; }

        [Column("idPersona")]
        public long IdPersona { get; set; }

        [Column("placaVehiculo")]
        [MaxLength(10)]
        public string? PlacaVehiculo { get; set; }

        [Column("total")]
        public double? Total { get; set; }

        [Column("fechaInicio")]
        public DateTime? FechaInicio { get; set; }

        [Column("fechaFinalizacion")]
        public DateTime? FechaFinalizacion { get; set; }

        [Column("observaciones")]
        public string? Observaciones { get; set; }

        // Propiedades de navegación
        [ForeignKey("IdTipoOrdenEstado")]
        public TipoOrdenEstado? TipoOrdenEstado { get; set; }

        [ForeignKey("IdPersona")]
        public Persona? Persona { get; set; }

        [ForeignKey("PlacaVehiculo")]
        public Vehiculo? Vehiculo { get; set; }

        // Colecciones de navegación
        public ICollection<OrdenServicio> OrdenServicios { get; set; } = new List<OrdenServicio>();
    }
}
