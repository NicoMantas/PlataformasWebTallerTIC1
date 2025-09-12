using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_TallerAutomoviles.Clases
{
    public class CEmpresa : Cliente
    {
        private long nit;
        private string representanteLegal;

        public CEmpresa(int id, string nombre, string email, long telefono, Vehiculo carro, long nit, string representanteLegal) : base(id, nombre, email, telefono, carro)
        {
            this.nit = nit;
            this.representanteLegal = representanteLegal;
        }

        public long Nit { get => nit; set => nit = value; }
        public string RepresentanteLegal { get => representanteLegal; set => representanteLegal = value; }
    }
}
