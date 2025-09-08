using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Interfaces;

namespace B_TallerAutomoviles.Clases.Validadores
{
    public class ValidarPersona : IValidarCliente, IValidarEmpleado
    {
        public bool ValidarCliente(Clientes.Cliente cliente)
        {
            if (cliente == null) return false;
            
            // Validar ID
            if (cliente.Id <= 0) return false;
            
            // Validar nombre
            if (string.IsNullOrWhiteSpace(cliente.Nombre) || cliente.Nombre.Length < 2) return false;
            
            // Validar email
            if (string.IsNullOrWhiteSpace(cliente.Email) || !cliente.Email.Contains("@")) return false;
            
            // Validar teléfono (debe tener al menos 7 dígitos)
            if (cliente.Telefono <= 0 || cliente.Telefono.ToString().Length < 7) return false;
            
            return true;
        }

        public bool ValidarEmpleado(Empleados.Empleado empleado)
        {
            if (empleado == null) return false;
            
            // Validar ID
            if (empleado.Id <= 0) return false;
            
            // Validar nombre
            if (string.IsNullOrWhiteSpace(empleado.Nombre) || empleado.Nombre.Length < 2) return false;
            
            // Validar apellido
            if (string.IsNullOrWhiteSpace(empleado.Apellido) || empleado.Apellido.Length < 2) return false;
            
            // Validar cédula (debe tener al menos 8 dígitos)
            if (empleado.Cedula <= 0 || empleado.Cedula.ToString().Length < 8) return false;
            
            // Validar salario
            if (empleado.Salario <= 0) return false;
            
            // Validar fecha de contratación
            if (empleado.FechaContratacion > DateTime.Now) return false;
            
            return true;
        }
    }
}
