using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Interfaces;

namespace B_TallerAutomoviles.Clases.Empleados
{
    public abstract class Empleado
    {
        private long id;
        private string nombre;
        private string apellido;
        private long cedula;
        private double salario;
        private DateTime fechaContratacion;

        protected readonly IValidarEmpleado validarEmpleado;

        public long Id { get => id; set => id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Apellido { get => apellido; set => apellido = value; }
        public long Cedula { get => cedula; set => cedula = value; }
        public double Salario { get => salario; set => salario = value; }
        public DateTime FechaContratacion { get => fechaContratacion; set => fechaContratacion = value; }

        public Empleado(long id, string nombre, string apellido, long cedula, double salario, DateTime fechaContratacion, IValidarEmpleado validarEmpleado)
        {
            this.Id = id;
            this.Nombre = nombre;
            this.Apellido = apellido;
            this.Cedula = cedula;
            this.Salario = salario;
            this.FechaContratacion = fechaContratacion;
            this.validarEmpleado = validarEmpleado;
            
            // Validar después de asignar los valores
            if (!validarEmpleado.ValidarEmpleado(this))
                throw new ArgumentException("Datos del empleado inválidos");
        }


    }
}
