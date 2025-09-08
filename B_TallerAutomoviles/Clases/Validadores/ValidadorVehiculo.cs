using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Clases.Vehiculos;
using B_TallerAutomoviles.Interfaces;

namespace B_TallerAutomoviles.Clases.Validadores
{
    public class ValidadorVehiculo : IValidarVehiculo
    {
        public bool ValidarVehiculo(Vehiculo vehiculo)
        {
            if (vehiculo == null) return false;
            
            // Validar placa
            if (string.IsNullOrWhiteSpace(vehiculo.Placa) || vehiculo.Placa.Length < 3) return false;
            
            // Validar marca
            if (string.IsNullOrWhiteSpace(vehiculo.Marca) || vehiculo.Marca.Length < 2) return false;
            
            // Validar modelo
            if (string.IsNullOrWhiteSpace(vehiculo.Modelo) || vehiculo.Modelo.Length < 2) return false;
            
            // Validar año (entre 1900 y año actual + 1)
            int añoActual = DateTime.Now.Year;
            if (vehiculo.Año < 1900 || vehiculo.Año > añoActual + 1) return false;
            
            // Validar propietario
            if (vehiculo.Propetario == null) return false;
            
            return true;
        }
    }
}
