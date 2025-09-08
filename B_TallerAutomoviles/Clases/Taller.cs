using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Clases.Clientes;
using B_TallerAutomoviles.Clases.Empleados;
using B_TallerAutomoviles.Clases.Vehiculos;
using B_TallerAutomoviles.Interfaces;

namespace B_TallerAutomoviles.Clases
{
    public class Taller
    {
        protected readonly IGestionOrdenTrabajo gestionOrdenTrabajoTaller;
        protected readonly IGestionTallerFactura gestionTallerFactura;
        protected readonly IGestionTallerMecanico gestionTallerMecanico;
        protected readonly IGestionTallerVehiculo gestionTallerVehiculo;
        protected readonly IGestionInventario gestionInventario;
        protected readonly IGestionRegistroCliente gestionRegistroCliente;
        // Atributos
        private string nombre;
        private string direccion;
        private Dictionary<string, Repuesto> inventario;
        private List<Empleado> empleados;
        private List<Cliente> clientes;
        private List<Vehiculo> vehiculos;
        private List<OrdenDeTrabajo> ordenesTrabajo;

        protected string Nombre { get => nombre; set => nombre = value; }
        protected string Direccion { get => direccion; set => direccion = value; }
        protected Dictionary<string, Repuesto> Inventario { get => inventario; set => inventario = value; }
        protected List<Empleado> Empleados { get => empleados; set => empleados = value; }
        protected List<Cliente> Clientes { get => clientes; set => clientes = value; }
        protected List<Vehiculo> Vehiculos { get => vehiculos; set => vehiculos = value; }
        protected List<OrdenDeTrabajo> OrdenesTrabajo { get => ordenesTrabajo; set => ordenesTrabajo = value; }

        // Constructor
        public Taller(string nombre, string direccion, IGestionOrdenTrabajo gestionOrdenTrabajoTaller, IGestionTallerFactura gestionTallerFactura, IGestionTallerMecanico gestionTallerMecanico, IGestionTallerVehiculo gestionTallerVehiculo, IGestionInventario gestionInventario, IGestionRegistroCliente gestionRegistroCliente)
        {
            this.Nombre = nombre;
            this.Direccion = direccion;
            this.Inventario = new Dictionary<string, Repuesto>();
            this.Empleados = new List<Empleado>();
            this.Clientes = new List<Cliente>();
            this.Vehiculos = new List<Vehiculo>();
            this.OrdenesTrabajo = new List<OrdenDeTrabajo>();
        }
    }
}
