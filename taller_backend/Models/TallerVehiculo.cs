using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller_backend.Models
{
    [Table("TallerVehiculo")]
    public class TallerVehiculo
    {
        [Key]
        [Column("idTaller")]
        public long IdTaller { get; set; }

        [Key]
        [Column("placaVehiculo")]
        [MaxLength(10)]
        public string PlacaVehiculo { get; set; } = string.Empty;

        [Column("fechaRegistro")]
        public DateTime? FechaRegistro { get; set; }

        // Propiedades de navegación
        [ForeignKey("IdTaller")]
        public Taller? Taller { get; set; }

        [ForeignKey("PlacaVehiculo")]
        public Vehiculo? Vehiculo { get; set; }
    }
}
