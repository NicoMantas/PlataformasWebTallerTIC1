using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Interfaces;

namespace B_TallerAutomoviles.Clases
{
    public class Factura
    {
        public enum MetodoPago
        {
            TarjetaDebito,
            TarjetaCredito,
            Efectivo,
            Transferencia
        }

        private long id;
        private DateOnly fecha;
        private double montoParcial;
        private float impuesto;
        private double montoTotal;
        private MetodoPago metodo;
        private List<OrdenDeTrabajo> ordenTrabajo;

        protected readonly IGestionFactura gestionarFactura;

        public long Id { get => id; set => id = value; }
        public DateOnly Fecha { get => fecha; set => fecha = value; }
        public double MontoParcial { get => montoParcial; set => montoParcial = value; }
        public float Impuesto { get => impuesto; set => impuesto = value; }
        public double MontoTotal { get => montoTotal; set => montoTotal = value; }
        public MetodoPago Metodo { get => metodo; set => metodo = value; }
        public List<OrdenDeTrabajo> OrdenTrabajo { get => ordenTrabajo; set => ordenTrabajo = value; }

        public Factura(long id, DateOnly fecha, double montoParcial, float impuesto, double montoTotal, MetodoPago metodo, IGestionFactura gestionarFactura)
        {
            this.Id = id;
            this.Fecha = fecha;
            this.MontoParcial = montoParcial;
            this.Impuesto = impuesto;
            this.MontoTotal = montoTotal;
            this.Metodo = metodo;
            this.OrdenTrabajo = new List<OrdenDeTrabajo>();
        }
    }
}
