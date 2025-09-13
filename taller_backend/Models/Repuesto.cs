using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace taller_backend.Models

{
    [Table ("Repuesto")]
    public class Repuesto
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("nombre")]
        public string? Nombre { get; set; }

        [Column("numeroParte")]
        public long? NumeroParte { get; set; }

        [Column("descripcion")]
        public string? Descripcion { get; set; }

        [Column("costoCompra")]
        public double? CostoCompra { get; set; }

        [Column("precioVenta")]
        public double? PrecioVenta { get; set; }

        [Column("cantidadStock")]
        public int? CantidadStock { get; set; }

        [Column("idProveedor")]
        public long? IdProveedor { get; set; }

        //navegación de repuestos
        [JsonIgnore]
        public Proveedor? Proveedor { get; set; } 
    }
}
