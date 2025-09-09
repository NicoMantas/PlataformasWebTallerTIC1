using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; //para mapear la clase con la tabla de la base de datos

namespace taller_backend.Models
{
    public class Taller
    {
        [Key]
        [Column("id")]
        public long Id { get; set; } = 1; // Valor por defecto

        [Column("nombre")]
        [MaxLength(100)]
        public string? Nombre { get; set; }

        [Column("direccion")]
        [MaxLength(200)]
        public string? Direccion { get; set; }

        [Column("telefono")]
        [MaxLength(20)]
        public string? Telefono { get; set; }

        [Column("email")]
        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [Column("horarioAtencion")]
        [MaxLength(100)]
        public string? HorarioAtencion { get; set; }

        [Column("fechaCreacion")]
        public DateTime? FechaCreacion { get; set; }

        // Colecciones de navegación
        public ICollection<TallerEmpleado> TallerEmpleados { get; set; } = new List<TallerEmpleado>();
        public ICollection<TallerCliente> TallerClientes { get; set; } = new List<TallerCliente>();
        public ICollection<TallerVehiculo> TallerVehiculos { get; set; } = new List<TallerVehiculo>();
        public ICollection<InventarioTaller> InventarioTaller { get; set; } = new List<InventarioTaller>();
    }
}
