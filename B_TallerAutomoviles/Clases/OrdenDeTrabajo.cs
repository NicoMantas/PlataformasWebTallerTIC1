using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Interfaces;

namespace B_TallerAutomoviles.Clases
{
    public class OrdenDeTrabajo : IOrdenTrabajo
    {
        public enum EstadoOrden
        {
            EnProgreso,
            Completada,
            Cancelada
        }

        private int id;
        private List<Servicio> servicios = new List<Servicio>();
        private DateTime fechaCreacion;
        private EstadoOrden estado;

        public int Id { get => id; set => id = value; }
        public List<Servicio> Servicios { get => servicios; set => servicios = value; }
        public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
        public EstadoOrden Estado { get => estado; set => estado = value; }

        public OrdenDeTrabajo()
        {
            this.Servicios = new List<Servicio>();
            this.FechaCreacion = DateTime.Now;
            this.Estado = EstadoOrden.EnProgreso;
        }

        public OrdenDeTrabajo(int id, List<Servicio> servicios, EstadoOrden estado)
        {
            this.Id = id;
            this.Servicios = servicios ?? new List<Servicio>();
            this.FechaCreacion = DateTime.Now;
            this.Estado = estado;
        }

        public void AgregarOrden(Servicio servicios)
        {
            //Se agrega el servicio a la lista de servicios en la orden
            if (servicios != null)
            {
                Servicios.Add(servicios);
            }
        }
    }
}
