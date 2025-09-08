using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Interfaces;
using B_TallerAutomoviles.Clases.Clientes;
using B_TallerAutomoviles.Clases.Vehiculos;
using B_TallerAutomoviles.Clases.Servicios;

namespace B_TallerAutomoviles.Clases.Gestion
{
    public class GestionEmpleado : IGestionAdministrador, IGestionMecanico, IGestionSecretaria, IRegistroEntradaSalidaVehiculo
    {
        private List<Vehiculo> vehiculosEnTaller = new List<Vehiculo>();
        private List<Servicio> serviciosActivos = new List<Servicio>();
        
        public void AgendarCita(Cliente cliente)
        {
            if (cliente == null) throw new ArgumentNullException(nameof(cliente));
            
            // Simular agendamiento de cita
            DateTime fechaCita = DateTime.Now.AddDays(1);
            Console.WriteLine($"Cita agendada para {cliente.Nombre} el {fechaCita:dd/MM/yyyy} a las {fechaCita:HH:mm}");
        }

        public void GestionarEmpleado()
        {
            // Método para gestionar empleados (contratación, despidos, cambios de salario, etc.)
            Console.WriteLine("Gestión de empleados iniciada");
        }

        public void GestionarReporteFinanciero()
        {
            // Generar reporte financiero del taller
            double ingresosTotales = serviciosActivos.Sum(s => s.CostoBase);
            double gastosOperativos = ingresosTotales * 0.6; // Estimación del 60% en gastos
            double gananciaNeta = ingresosTotales - gastosOperativos;
            
            Console.WriteLine("=== REPORTE FINANCIERO ===");
            Console.WriteLine($"Ingresos totales: ${ingresosTotales:F2}");
            Console.WriteLine($"Gastos operativos: ${gastosOperativos:F2}");
            Console.WriteLine($"Ganancia neta: ${gananciaNeta:F2}");
        }

        public string ListarServiciosActivos(List<Servicio> servicio)
        {
            if (servicio == null || servicio.Count == 0)
            {
                return "No hay servicios activos";
            }
            
            var reporte = new StringBuilder();
            reporte.AppendLine("=== SERVICIOS ACTIVOS ===");
            
            foreach (var s in servicio)
            {
                reporte.AppendLine($"ID: {s.Id} - {s.Descripcion} - Estado: {s.Estado_enum} - Costo: ${s.CostoBase:F2}");
            }
            
            return reporte.ToString();
        }

        public void RegistrarEntradaVehiculo(Vehiculo vehiculo)
        {
            if (vehiculo == null) throw new ArgumentNullException(nameof(vehiculo));
            
            if (!vehiculosEnTaller.Contains(vehiculo))
            {
                vehiculosEnTaller.Add(vehiculo);
                Console.WriteLine($"Vehículo {vehiculo.Marca} {vehiculo.Modelo} ({vehiculo.Placa}) registrado en el taller");
            }
            else
            {
                Console.WriteLine($"El vehículo {vehiculo.Placa} ya está registrado en el taller");
            }
        }

        public void RegistrarSalidaVehiculo(Vehiculo vehiculo)
        {
            if (vehiculo == null) throw new ArgumentNullException(nameof(vehiculo));
            
            if (vehiculosEnTaller.Contains(vehiculo))
            {
                vehiculosEnTaller.Remove(vehiculo);
                Console.WriteLine($"Vehículo {vehiculo.Marca} {vehiculo.Modelo} ({vehiculo.Placa}) retirado del taller");
            }
            else
            {
                Console.WriteLine($"El vehículo {vehiculo.Placa} no está registrado en el taller");
            }
        }
    }
}
