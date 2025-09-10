using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller_backend.Models
{
    [Table("VehiculoElectrico")]
    public class VehiculoElectrico
    {
        [Key]
        [Column("placa")]
        [MaxLength(10)]
        public string Placa { get; set; } = string.Empty;

        [Column("capacidadBateria")]
        public int? CapacidadBateria { get; set; }

        // Propiedades de navegación
        [ForeignKey("Placa")]
        public Vehiculo? Vehiculo { get; set; }
    }
}
