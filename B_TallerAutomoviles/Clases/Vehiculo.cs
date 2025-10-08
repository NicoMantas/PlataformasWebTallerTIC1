using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_TallerAutomoviles.Clases
{
    public abstract class Vehiculo
    {
        private int id;
        private string placa;
        private string marca;
        private string modelo;
        private int año;

        public int Id { get => id; set => id = value; }
        public string Placa { get => placa; set => placa = value; }
        public string Marca { get => marca; set => marca = value; }
        public string Modelo { get => modelo; set => modelo = value; }
        public int Año { get => año; set => año = value; }

        protected Vehiculo(int id, string placa, string marca, string modelo, int año)
        {
            this.Id = id;
            this.Placa = placa;
            this.Marca = marca;
            this.Modelo = modelo;
            this.Año = año;
        }
    }
}
