using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_TallerAutomoviles.Clases
{
    public class Proveedor
    {
        private int id;
        private string nombre;
        private string contacto;

        public Proveedor(int id, string nombre, string contacto)
        {
            this.Id = id;
            this.Nombre = nombre;
            this.Contacto = contacto;
        }

        public int Id { get => id; set => id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Contacto { get => contacto; set => contacto = value; }
    }
}
