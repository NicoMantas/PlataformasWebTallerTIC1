using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Interfaces;

namespace B_TallerAutomoviles.Clases.Clientes
{
    public class ClienteNatural : Cliente
    {
        private long cedula;
        private string apellido;

        public ClienteNatural(long id, string nombre, string email, long telefono, IValidarCliente validarCliente, long cedula, string apellido) 
            : base(id, nombre, email, telefono, validarCliente)
        {
            this.Cedula = cedula;
            this.Apellido = apellido;
        }

        public long Cedula { get => cedula; set => cedula = value; }
        public string Apellido { get => apellido; set => apellido = value; }
    }
}
