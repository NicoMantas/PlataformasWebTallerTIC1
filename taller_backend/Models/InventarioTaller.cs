using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller_backend.Models
{
    [Table("InventarioTaller")]
    public class InventarioTaller
    {
        [Key]
        [Column("idTaller")]
        public long IdTaller { get; set; }

        [Key]
        [Column("idRepuesto")]
        public long IdRepuesto { get; set; }

        [Column("cantidad")]
        public int Cantidad { get; set; }

        [Column("stockMinimo")]
        public int StockMinimo { get; set; }

        // Propiedades de navegación con ForeignKey 
        [ForeignKey(nameof(IdRepuesto))]
        public Repuesto? Repuesto { get; set; }

        [ForeignKey(nameof(IdTaller))]
        public Taller? Taller { get; set; }

    }
}
