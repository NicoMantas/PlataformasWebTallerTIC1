using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Interfaces;
using B_TallerAutomoviles.Clases.Vehiculos;
using B_TallerAutomoviles.Clases.Empleados;

namespace B_TallerAutomoviles.Clases.Servicios
{
    public class Servicio
    {
        protected readonly IGestionServicio gestionarServicio;
        public enum Estado { Enproceso, Pendiente, Terminado}

        private long id;
        private Vehiculo vehiculo;
        private string descripcion;
        private double costoBase;
        private int tiempoEstimado;
        private List<Mecanico> mecanicosAsignados;
        private Estado estado_enum;

        public long Id { get => id; set => id = value; }
        public Vehiculo Vehiculo { get => vehiculo; set => vehiculo = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public double CostoBase { get => costoBase; set => costoBase = value; }
        public int TiempoEstimado { get => tiempoEstimado; set => tiempoEstimado = value; }
        public List<Mecanico> MecanicosAsignados { get => mecanicosAsignados; set => mecanicosAsignados = value; }
        public Estado Estado_enum { get => estado_enum; set => estado_enum = value; }

        public Servicio(long id, Vehiculo vehiculo, string descripcion, double costoBase, int tiempoEstimado, Estado estado, IGestionServicio gestionarServicio)
        {
            this.Id = id;
            this.Vehiculo = vehiculo;
            this.Descripcion = descripcion;
            this.CostoBase = costoBase;
            this.TiempoEstimado = tiempoEstimado;
            this.MecanicosAsignados = new List<Mecanico>();
            this.Estado_enum = estado;
        }
    }
}
