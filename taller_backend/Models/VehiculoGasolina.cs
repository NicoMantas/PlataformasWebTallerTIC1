using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller_backend.Models
{
    [Table("VehiculoGasolina")]
    public class VehiculoGasolina
    {
        [Key]
        [Column("placa")]
        [MaxLength(10)]
        public string Placa { get; set; } = string.Empty;

        [Column("cilindraje")]
        public int? Cilindraje { get; set; }

        // Propiedades de navegación
        [ForeignKey("Placa")]
        public Vehiculo? Vehiculo { get; set; }

    }
}
