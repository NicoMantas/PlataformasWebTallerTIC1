using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Interfaces;

namespace B_TallerAutomoviles.Clases.Empleados
{
    public class Administrador : Empleado
    {
        protected readonly IGestionAdministrador gestionarAdministrador;
        public Administrador(long id, string nombre, string apellido, long cedula, double salario, DateTime fechaContratacion, IValidarEmpleado validarEmpleado, IGestionAdministrador gestionarAdministrador) 
            : base(id, nombre, apellido, cedula, salario, fechaContratacion, validarEmpleado)
        {
            this.gestionarAdministrador = gestionarAdministrador;
        }
    }
}
