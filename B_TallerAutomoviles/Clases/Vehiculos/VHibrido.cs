using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Clases.Clientes;
using B_TallerAutomoviles.Interfaces;

namespace B_TallerAutomoviles.Clases.Vehiculos
{
    public class VHibrido : Vehiculo
    {
        private int cilindraje;
        private int capacidadBateria;

        public VHibrido(string placa, string marca, string modelo, int año, Cliente propetario, IValidarVehiculo validarVehiculo, int cilindraje, int capacidadBateria)
            : base(placa, marca, modelo, año, propetario, validarVehiculo)
        {
            this.Cilindraje = cilindraje;
            this.CapacidadBateria = capacidadBateria;
        }

        public int Cilindraje { get => cilindraje; set => cilindraje = value; }
        public int CapacidadBateria { get => capacidadBateria; set => capacidadBateria = value; }
    }
}
