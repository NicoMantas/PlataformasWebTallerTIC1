using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Clases.Clientes;
using B_TallerAutomoviles.Interfaces;

namespace B_TallerAutomoviles.Clases.Vehiculos
{
    public abstract class Vehiculo
    {
        private string placa;
        private string marca;
        private string modelo;
        private int año;
        private Cliente propetario;

        protected readonly IValidarVehiculo validarVehiculo;

        public string Placa { get => placa; set => placa = value; }
        public string Marca { get => marca; set => marca = value; }
        public string Modelo { get => modelo; set => modelo = value; }
        public int Año { get => año; set => año = value; }
        public Cliente Propetario { get => propetario; set => propetario = value; }

        protected Vehiculo(string placa, string marca, string modelo, int año, Cliente propetario, IValidarVehiculo validarVehiculo)
        {
            this.Placa = placa;
            this.Marca = marca;
            this.Modelo = modelo;
            this.Año = año;
            this.Propetario = propetario;
            this.validarVehiculo = validarVehiculo;
            
            // Validar después de asignar los valores
            if (!validarVehiculo.ValidarVehiculo(this))
                throw new ArgumentException("Datos del vehículo inválidos");
        }
    }
}
