using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_TallerAutomoviles.Clases
{
    public abstract class Servicio
    {
        public enum Estado
        {
            Pendiente,
            EnProceso,
            Terminado
        }

        private int id;
        private Cliente cliente;
        private List<Empleado> mecanico;
        private Estado estado1;


        protected Servicio()
        {
            this.Mecanico = new List<Empleado>();
            this.Estado1 = Estado.Pendiente;
        }

        protected Servicio(int id, Cliente cliente, List<Empleado> mecanico, Estado estado)
        {
            this.Id = id;
            this.Cliente = cliente;
            this.Mecanico = mecanico;
            this.Estado1 = estado;
        }

        public int Id { get => id; set => id = value; }
        public Cliente Cliente { get => cliente; set => cliente = value; }
        public List<Empleado> Mecanico { get => mecanico; set => mecanico = value; }
        public Estado Estado1 { get => estado1; set => estado1 = value; }
    }
}
