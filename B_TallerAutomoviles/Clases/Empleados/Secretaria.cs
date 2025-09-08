using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Interfaces;

namespace B_TallerAutomoviles.Clases.Empleados
{
    public class Secretaria : Empleado
    {
        protected readonly IGestionSecretaria gestionSecretaria;
        protected readonly IRegistroEntradaSalidaVehiculo registroEntradaSalidaVehiculo;
        public Secretaria(long id, string nombre, string apellido, long cedula, double salario, DateTime fechaContratacion, IValidarEmpleado validarEmpleado, IGestionSecretaria gestionSecretaria, IRegistroEntradaSalidaVehiculo registroEntradaSalidaVehiculo)
            : base(id, nombre, apellido, cedula, salario, fechaContratacion, validarEmpleado)
        {
            this.gestionSecretaria = gestionSecretaria;
            this.registroEntradaSalidaVehiculo = registroEntradaSalidaVehiculo;
        }
    }
}
