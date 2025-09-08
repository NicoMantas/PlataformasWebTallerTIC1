using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Interfaces;
using B_TallerAutomoviles.Clases.Servicios;

namespace B_TallerAutomoviles.Clases.Gestion
{
    public class GestionServicioFactura : IGestionFactura, IGestionServicio
    {
        private static long contadorFacturas = 1;
        private double costoAcumulado = 0;
        private List<Servicio> serviciosActivos = new List<Servicio>();
        
        public double CalcularCostoFactura()
        {
            // Calcular el costo total de todos los servicios
            double costoTotal = serviciosActivos.Sum(s => s.CostoBase);
            
            // Aplicar impuesto (19% IVA)
            double impuesto = costoTotal * 0.19;
            
            return costoTotal + impuesto;
        }

        public double CalcularCostoServicio()
        {
            // Retornar el costo acumulado de servicios
            return costoAcumulado;
        }

        public void FinalizarServicio()
        {
            // Marcar todos los servicios activos como terminados
            foreach (var servicio in serviciosActivos)
            {
                servicio.Estado_enum = Servicio.Estado.Terminado;
            }
            
            Console.WriteLine("Servicios finalizados correctamente");
        }

        public Factura ImprimirFactura()
        {
            if (serviciosActivos.Count == 0)
            {
                throw new InvalidOperationException("No hay servicios para facturar");
            }
            
            // Calcular montos
            double montoParcial = serviciosActivos.Sum(s => s.CostoBase);
            float impuesto = 0.19f;
            double montoTotal = montoParcial * (1 + impuesto);
            
            // Crear factura
            var factura = new Factura(
                contadorFacturas++,
                DateOnly.FromDateTime(DateTime.Now),
                montoParcial,
                impuesto,
                montoTotal,
                Factura.MetodoPago.Efectivo,
                null
            );
            
            Console.WriteLine($"Factura #{factura.Id} impresa - Total: ${factura.MontoTotal:F2}");
            
            return factura;
        }

        public void IniciarServicio()
        {
            // Cambiar el estado de los servicios a "En proceso"
            foreach (var servicio in serviciosActivos.Where(s => s.Estado_enum == Servicio.Estado.Pendiente))
            {
                servicio.Estado_enum = Servicio.Estado.Enproceso;
                costoAcumulado += servicio.CostoBase;
            }
            
            Console.WriteLine("Servicios iniciados correctamente");
        }
    }
}
