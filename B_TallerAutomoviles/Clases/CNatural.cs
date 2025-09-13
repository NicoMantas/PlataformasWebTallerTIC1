using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_TallerAutomoviles.Clases
{
    public class CNatural : Cliente
    {
        private long cedula;
        private string apellido;

        public CNatural(int id, string nombre, string email, long telefono, Vehiculo carro, long cedula, string apellido) : base(id, nombre, email, telefono, carro)
        {
            this.Cedula = cedula;
            this.Apellido = apellido;
        }

        public long Cedula { get => cedula; set => cedula = value; }
        public string Apellido { get => apellido; set => apellido = value; }
    }
}
