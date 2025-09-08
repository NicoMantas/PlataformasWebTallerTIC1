using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Clases.Clientes;
using B_TallerAutomoviles.Interfaces;

namespace B_TallerAutomoviles.Clases.Vehiculos
{
    public class VElectrico : Vehiculo
    {
        private int capacidadBateria;

        public VElectrico(string placa, string marca, string modelo, int año, Cliente propetario, IValidarVehiculo validarVehiculo, int capacidadBateria)
            : base(placa, marca, modelo, año, propetario, validarVehiculo)
        {
            this.CapacidadBateria = capacidadBateria;
        }

        public int CapacidadBateria { get => capacidadBateria; set => capacidadBateria = value; }
    }
}
