using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_TallerAutomoviles.Clases
{
    public class VGasolina : Vehiculo
    {
        private int cilindraje;

        public VGasolina(int id, string placa, string marca, string modelo, int año, int cilindraje) : base(id, placa, marca, modelo, año)
        {
            this.Cilindraje = cilindraje;
        }

        public int Cilindraje { get => cilindraje; set => cilindraje = value; }
    }
}
