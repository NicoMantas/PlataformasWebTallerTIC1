using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Interfaces;

namespace B_TallerAutomoviles.Clases
{
    public class Revision : Servicio, IRevision
    {
        private string descripcion = string.Empty;

        public Revision(int id, Cliente cliente, List<Empleado> mecanico, Estado estado, string descripcion) : base(id, cliente, mecanico, estado)
        {
            this.Descripcion = descripcion;
        }

        public string Descripcion { get => descripcion; set => descripcion = value; }

        public string RealizarRevision()
        {
            //Este metodo realizara una revision y regresara una descripcion
            StringBuilder reporte = new StringBuilder();
            reporte.AppendLine("=== REPORTE DE REVISIÓN ===");
            reporte.AppendLine($"ID Revisión: {Id}");
            reporte.AppendLine($"Cliente: {Cliente.Nombre}");
            reporte.AppendLine($"Vehículo: {Cliente.Carro.Marca} {Cliente.Carro.Modelo} - {Cliente.Carro.Placa}");
            reporte.AppendLine($"Tipo de Vehículo: {Cliente.Carro.GetType().Name}");
            reporte.AppendLine($"Estado: {Estado1}");
            reporte.AppendLine("----------------------------------------");
            reporte.AppendLine("Descripción de la Revisión:");
            reporte.AppendLine(Descripcion);
            reporte.AppendLine("----------------------------------------");
            
            // Determinar tipo de revisión según el tipo de vehículo
            string tipoVehiculo = Cliente.Carro.GetType().Name;
            string tipoRevision = "";
            
            switch (tipoVehiculo)
            {
                case "VElectrico":
                    tipoRevision = "Revisión de sistema eléctrico, batería y componentes electrónicos";
                    break;
                case "VGasolina":
                    tipoRevision = "Revisión de motor, sistema de combustible y emisiones";
                    break;
                case "VHibrido":
                    tipoRevision = "Revisión completa de sistemas híbridos (motor + eléctrico)";
                    break;
                default:
                    tipoRevision = "Revisión general del vehículo";
                    break;
            }
            
            reporte.AppendLine($"Tipo de Revisión: {tipoRevision}");
            reporte.AppendLine("----------------------------------------");
            reporte.AppendLine("Mecánicos Asignados:");
            if (Mecanico != null && Mecanico.Count > 0)
            {
                foreach (var mecanico in Mecanico)
                {
                    reporte.AppendLine($"- {mecanico.Nombre}");
                }
            }
            else
            {
                reporte.AppendLine("- No hay mecánicos asignados");
            }
            
            reporte.AppendLine("========================================");
            
            return reporte.ToString();
        }
    }
}
