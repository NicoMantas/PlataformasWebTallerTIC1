using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_TallerAutomoviles.Clases
{
    public class VHibrido : Vehiculo
    {
        private int cilindraje;
        private int capacidadBateria;
        
        public VHibrido(int id, string placa, string marca, string modelo, int año, int cilindraje, int capacidadBateria) : base(id, placa, marca, modelo, año)
        {
            this.Cilindraje = cilindraje;
            this.CapacidadBateria = capacidadBateria;
        }

        public int Cilindraje { get => cilindraje; set => cilindraje = value; }
        public int CapacidadBateria { get => capacidadBateria; set => capacidadBateria = value; }
    }
}
