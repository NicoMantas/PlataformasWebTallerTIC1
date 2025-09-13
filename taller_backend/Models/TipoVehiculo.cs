using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; //para mapear la clase con la tabla de la base de datos
using System.Text.Json.Serialization;

namespace taller_backend.Models
{

    [Table("TipoVehiculo")]
    public class TipoVehiculo
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("descripcion")]
        [MaxLength(20)]
        public string? Descripcion { get; set; }

        // Colección de navegación
        [JsonIgnore]
        public ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
    }
}
