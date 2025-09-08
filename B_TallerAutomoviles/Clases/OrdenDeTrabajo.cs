using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Clases.Clientes;
using B_TallerAutomoviles.Clases.Servicios;
using B_TallerAutomoviles.Clases.Vehiculos;
using B_TallerAutomoviles.Interfaces;

namespace B_TallerAutomoviles.Clases
{
    public class OrdenDeTrabajo
    {
        public enum estadoOrden { EnProceso, Completada, Cancelada}

        private long id;
        private DateTime fechaCreacion;
        private estadoOrden estado;
        private Cliente cliente;
        private Vehiculo vehiculo;
        private List<Servicio> servicio;

        protected readonly IGestionOrden gestionarOrdenDeTrabajo;

        public long Id { get => id; set => id = value; }
        public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
        public estadoOrden Estado { get => estado; set => estado = value; }
        public Cliente Cliente { get => cliente; set => cliente = value; }
        public Vehiculo Vehiculo { get => vehiculo; set => vehiculo = value; }
        public List<Servicio> Servicio { get => servicio; set => servicio = value; }

        public OrdenDeTrabajo(long id, DateTime fechaCreacion, estadoOrden estado, Cliente cliente, Vehiculo vehiculo, IGestionOrden gestionarOrdenDeTrabajo)
        {
            this.Id = id;
            this.FechaCreacion = fechaCreacion;
            this.Estado = estado;
            this.Cliente = cliente;
            this.Vehiculo = vehiculo;
            this.Servicio = new List<Servicio>();
        }
    }
}
