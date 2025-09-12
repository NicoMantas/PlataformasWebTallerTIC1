using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_TallerAutomoviles.Clases
{
    public abstract class Cliente
    {
        private int id;
        private string nombre;
        private string email;
        private long telefono;
        private Vehiculo carro;

        public int Id { get => id; set => id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Email { get => email; set => email = value; }
        public long Telefono { get => telefono; set => telefono = value; }
        public Vehiculo Carro { get => carro; set => carro = value; }

        public Cliente(int id, string nombre, string email, long telefono, Vehiculo carro)
        {
            this.Id = id;
            this.Nombre = nombre;
            this.Email = email;
            this.Telefono = telefono;
            this.Carro = carro;
        }
    }
}
