using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller_backend.Models
{
    [Table("Vehiculo")]
    public class Vehiculo
    {
        [Key]
        [Column("placa")]
        [MaxLength(10)]
        public string Placa { get; set; } = string.Empty;

        [Column("marca")]
        [MaxLength(50)]
        public string? Marca { get; set; }

        [Column("modelo")]
        [MaxLength(50)]
        public string? Modelo { get; set; }

        [Column("año")]
        public int? Año { get; set; }

        [Column("idPersona")]
        public long? IdPersona { get; set; }

        [Column("idTipoVehiculo")]
        [MaxLength(20)]
        public long? IdTipoVehiculo { get; set; }

        [Column("fechaCreacion")]
        public DateTime? FechaCreacion { get; set; }

        // Propiedades de navegación
        [ForeignKey("IdPersona")]
        public Persona? Persona { get; set; }

        [ForeignKey("IdTipoVehiculo")] // Corregido para que coincida con el nombre de la propiedad
        public TipoVehiculo? TipoVehiculo { get; set; }

    }
}
