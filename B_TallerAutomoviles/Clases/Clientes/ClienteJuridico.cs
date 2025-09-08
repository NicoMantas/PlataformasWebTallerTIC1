using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Interfaces;

namespace B_TallerAutomoviles.Clases.Clientes
{
    public class ClienteJuridico : Cliente
    {
        private long nit;
        private string representanteLegal;

        public ClienteJuridico(long id, string nombre, string email, long telefono, IValidarCliente validarCliente, long nit, string representanteLegal) : base(id, nombre, email, telefono, validarCliente)
        {
            this.Nit = nit;
            this.RepresentanteLegal = representanteLegal;
        }

        public long Nit { get => nit; set => nit = value; }
        public string RepresentanteLegal { get => representanteLegal; set => representanteLegal = value; }
    }
}
