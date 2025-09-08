using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Interfaces;

namespace B_TallerAutomoviles.Clases
{
    public class Proveedor
    {
        private long id;
        private string nombre;
        private long contacto;

        protected readonly IValidarProveedor validarProveedor;

        public Proveedor(long id, string nombre, long contacto)
        {
            this.Id = id;
            this.Nombre = nombre;
            this.Contacto = contacto;
            
            // Validar después de asignar los valores
            if (!validarProveedor.ValidarProveedor(this))
                throw new ArgumentException("Datos del proveedor inválidos");
        }

        public long Id { get => id; set => id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public long Contacto { get => contacto; set => contacto = value; }
    }
}
