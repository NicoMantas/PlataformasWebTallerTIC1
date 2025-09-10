using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; //para mapear la clase con la tabla de la base de datos


namespace taller_backend.Models
{
    [Table("Proveedor")] //este atributo es para mapear la clase con la tabla de la base de datos
    public class Proveedor
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("nombre")]
        public string? Nombre { get; set; }

        [Column("contacto")]
        public string? Contacto { get; set; }

        // navegación (colección)
       public ICollection<Repuesto> Repuestos { get; set; } = new List<Repuesto>(); //inicializamos la colección para evitar errores de referencia nula

    }
}
