using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Interfaces;

namespace B_TallerAutomoviles.Clases.Clientes
{
    public abstract class Cliente
    {
        protected readonly IValidarCliente validarCliente;

        private long id;
        private string nombre;
        private string email;
        private long telefono;

        public long Id { get => id; set => id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Email { get => email; set => email = value; }
        public long Telefono { get => telefono; set => telefono = value; }

        protected Cliente(long id, string nombre, string email, long telefono, IValidarCliente validarCliente)
        {
            this.Id = id;
            this.Nombre = nombre;
            this.Email = email;
            this.Telefono = telefono;
            this.validarCliente = validarCliente;
            
            // Validar después de asignar los valores
            if (!validarCliente.ValidarCliente(this))
                throw new ArgumentException("Datos del cliente inválidos");
        }


    }
}
