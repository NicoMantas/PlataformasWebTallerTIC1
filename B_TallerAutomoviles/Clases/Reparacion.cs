using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Interfaces;

namespace B_TallerAutomoviles.Clases
{
    public class Reparacion : Servicio, IReparacion
    {
        private List<Repuesto> repuestos;

        public Reparacion(int id, Cliente cliente, List<Empleado> mecanico, Estado estado, List<Repuesto> repuestos) : base(id, cliente, mecanico, estado)
        {
            this.Repuestos = repuestos;
        }

        public List<Repuesto> Repuestos { get => repuestos; set => repuestos = value; }

        public string RealizarReparacion()
        {
            //Este metodo realizara una reparacion y usara una lista de repuestos solo hara una funcion de reparacion en texto, todo depende el tipo de auto
            StringBuilder reporte = new StringBuilder();
            reporte.AppendLine("=== REPORTE DE REPARACIÓN ===");
            reporte.AppendLine($"ID Reparación: {Id}");
            reporte.AppendLine($"Cliente: {Cliente.Nombre}");
            reporte.AppendLine($"Vehículo: {Cliente.Carro.Marca} {Cliente.Carro.Modelo} - {Cliente.Carro.Placa}");
            reporte.AppendLine($"Tipo de Vehículo: {Cliente.Carro.GetType().Name}");
            reporte.AppendLine($"Estado: {Estado1}");
            reporte.AppendLine("----------------------------------------");
            
            // Determinar tipo de reparación según el tipo de vehículo
            string tipoVehiculo = Cliente.Carro.GetType().Name;
            string tipoReparacion = "";
            
            switch (tipoVehiculo)
            {
                case "VElectrico":
                    tipoReparacion = "Reparación de sistema eléctrico y batería";
                    break;
                case "VGasolina":
                    tipoReparacion = "Reparación de motor de combustión interna";
                    break;
                case "VHibrido":
                    tipoReparacion = "Reparación de sistema híbrido (motor + eléctrico)";
                    break;
                default:
                    tipoReparacion = "Reparación general del vehículo";
                    break;
            }
            
            reporte.AppendLine($"Tipo de Reparación: {tipoReparacion}");
            reporte.AppendLine("Repuestos Utilizados:");
            
            if (Repuestos != null && Repuestos.Count > 0)
            {
                foreach (var repuesto in Repuestos)
                {
                    reporte.AppendLine($"- {repuesto.Nombre} (Serie: {repuesto.Numero_serie}) - ${repuesto.Precio:F2}");
                }
            }
            else
            {
                reporte.AppendLine("- No se utilizaron repuestos");
            }
            
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
