using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_TallerAutomoviles.Clases
{
    public class Empleado
    {
        public enum TipoEmpleado
        {
            Administrador,
            Secretaria,
            Mecanico
        }

        private int id;
        private string nombre;
        private string apellido;
        private long cedula;
        private double salario;
        private DateTime fechaContratacion;
        private TipoEmpleado tipo;

        public int Id { get => id; set => id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Apellido { get => apellido; set => apellido = value; }
        public long Cedula { get => cedula; set => cedula = value; }
        public double Salario { get => salario; set => salario = value; }
        public DateTime FechaContratacion { get => fechaContratacion; set => fechaContratacion = value; }
        public TipoEmpleado Tipo { get => tipo; set => tipo = value; }

        public Empleado()
        {
            this.FechaContratacion = DateTime.Now;
        }

        public Empleado(int id, string nombre, string apellido, long cedula, double salario, TipoEmpleado tipo)
        {
            this.Id = id;
            this.Nombre = nombre;
            this.Apellido = apellido;
            this.Cedula = cedula;
            this.Salario = salario;
            this.FechaContratacion = DateTime.Now;
            this.Tipo = tipo;
        }
    }
}
