using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Interfaces;

namespace B_TallerAutomoviles.Clases.Empleados
{
    public class Mecanico : Empleado
    {
        private string especialidad;
        private List<string> tareasAsignadas;

        protected readonly IGestionMecanico gestionarMecanico;

        public Mecanico(long id, string nombre, string apellido, long cedula, double salario, DateTime fechaContratacion, IValidarEmpleado validarEmpleado, string especialidad, IGestionMecanico gestionarMecanico) 
            : base(id, nombre, apellido, cedula, salario, fechaContratacion, validarEmpleado)
        {
            this.Especialidad = especialidad;
            this.TareasAsignadas = new List<string>();
        }

        public string Especialidad { get => especialidad; set => especialidad = value; }
        public List<string> TareasAsignadas { get => tareasAsignadas; set => tareasAsignadas = value; }
    }
}
