using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_TallerAutomoviles.Clases
{
    public class VElectrico : Vehiculo
    {
        private int capacidadBateria;

        public VElectrico(int id, string placa, string marca, string modelo, int año, int capacidadBateria) : base(id, placa, marca, modelo, año)
        {
            this.CapacidadBateria = capacidadBateria;
        }

        public int CapacidadBateria { get => capacidadBateria; set => capacidadBateria = value; }
    }
}
