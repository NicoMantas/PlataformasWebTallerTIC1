using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Interfaces;

namespace B_TallerAutomoviles.Clases
{
    public class Factura : IFactura
    {
        public enum MetodoPago
        {
            TarjetaDebito,
            TarjetaCredito,
            Transferencia,
            Efectivo
        }

        private int id;
        private DateTime fechaEmision;
        private Cliente cliente;
        private OrdenDeTrabajo ordenTrabajo;
        private double montoParcial;
        private float impuesto;
        private double montoTotal;
        private MetodoPago metodoPagoFactura;

        public int Id { get => id; set => id = value; }
        public DateTime FechaEmision { get => fechaEmision; set => fechaEmision = value; }
        public Cliente Cliente { get => cliente; set => cliente = value; }
        public OrdenDeTrabajo OrdenTrabajo { get => ordenTrabajo; set => ordenTrabajo = value; }
        public double MontoParcial { get => montoParcial; set => montoParcial = value; }
        public float Impuesto { get => impuesto; set => impuesto = value; }
        public double MontoTotal { get => montoTotal; set => montoTotal = value; }
        public MetodoPago MetodoPagoFactura { get => metodoPagoFactura; set => metodoPagoFactura = value; }

        public Factura(int id, Cliente cliente, OrdenDeTrabajo ordenTrabajo, double montoParcial, float impuesto, double montoTotal, MetodoPago metodoPago)
        {
            this.Id = id;
            this.FechaEmision = DateTime.Now ;
            this.Cliente = cliente;
            this.OrdenTrabajo = ordenTrabajo;
            this.MontoParcial = montoParcial;
            this.Impuesto = impuesto;
            this.MontoTotal = montoTotal;
            this.MetodoPagoFactura = metodoPago;
        }

        public string GenerarFactura()
        {
            //Generar un estilo de string para la factura y tener en cuenta el metodo de pago
            StringBuilder factura = new StringBuilder();
            factura.AppendLine("=== FACTURA DE TALLER AUTOMOTRIZ ===");
            factura.AppendLine($"ID Factura: {Id}");
            factura.AppendLine($"Fecha de Emisión: {FechaEmision:dd/MM/yyyy HH:mm}");
            factura.AppendLine($"Cliente: {Cliente.Nombre}");
            factura.AppendLine($"Email: {Cliente.Email}");
            factura.AppendLine($"Teléfono: {Cliente.Telefono}");
            factura.AppendLine($"Vehículo: {Cliente.Carro.Marca} {Cliente.Carro.Modelo} - {Cliente.Carro.Placa}");
            factura.AppendLine($"Orden de Trabajo ID: {OrdenTrabajo.Id}");
            factura.AppendLine($"Estado: {OrdenTrabajo.Estado}");
            factura.AppendLine("----------------------------------------");
            factura.AppendLine($"Monto Parcial: ${MontoParcial:F2}");
            factura.AppendLine($"Impuesto ({Impuesto * 100:F1}%): ${(MontoParcial * Impuesto):F2}");
            factura.AppendLine("----------------------------------------");
            factura.AppendLine($"TOTAL: ${MontoTotal:F2}");
            factura.AppendLine($"Método de Pago: {MetodoPagoFactura}");
            factura.AppendLine("========================================");
            
            return factura.ToString();
        }

        public double CalcularMontoTotal()
        {
            //Calcular el monto total en base de monto parcial, impuesto y lo asignara al MontoTotal
            double impuestoCalculado = MontoParcial * Impuesto;
            MontoTotal = MontoParcial + impuestoCalculado;
            return MontoTotal;
        }
    }
}
