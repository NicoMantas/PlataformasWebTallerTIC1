using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Clases.Clientes;
using B_TallerAutomoviles.Clases.Empleados;
using B_TallerAutomoviles.Clases.Vehiculos;
using B_TallerAutomoviles.Interfaces;

namespace B_TallerAutomoviles.Clases.Gestion
{
    public class GestionTaller : IGestionTallerFactura, IGestionTallerMecanico, IGestionTallerVehiculo, IGestionInventario, IGestionRegistroCliente
    {
        private static long contadorFacturas = 1;
        
        public void AsignarMecanico(OrdenDeTrabajo ordenDeTrabajo, Mecanico mecanico)
        {
            if (ordenDeTrabajo == null) throw new ArgumentNullException(nameof(ordenDeTrabajo));
            if (mecanico == null) throw new ArgumentNullException(nameof(mecanico));
            
            // Aquí se asignaría el mecánico a la orden de trabajo
            // Por ahora, solo registramos la asignación
            Console.WriteLine($"Mecánico {mecanico.Nombre} {mecanico.Apellido} asignado a la orden #{ordenDeTrabajo.Id}");
        }

        public Factura GenerarFactura(OrdenDeTrabajo orden)
        {
            if (orden == null) throw new ArgumentNullException(nameof(orden));
            
            // Calcular el monto total de los servicios
            double montoParcial = 0;
            if (orden.Servicio != null)
            {
                montoParcial = orden.Servicio.Sum(s => s.CostoBase);
            }
            
            // Aplicar impuesto (19% IVA)
            float impuesto = 0.19f;
            double montoTotal = montoParcial * (1 + impuesto);
            
            // Crear la factura
            var factura = new Factura(
                contadorFacturas++,
                DateOnly.FromDateTime(DateTime.Now),
                montoParcial,
                impuesto,
                montoTotal,
                Factura.MetodoPago.Efectivo, // Por defecto
                null // IGestionFactura se pasaría aquí si fuera necesario
            );
            
            // Agregar la orden a la factura
            factura.OrdenTrabajo.Add(orden);
            
            Console.WriteLine($"Factura #{factura.Id} generada - Total: ${factura.MontoTotal:F2}");
            
            return factura;
        }

        public void GestionarInventario(Repuesto repuesto, int cantidad)
        {
            if (repuesto == null) throw new ArgumentNullException(nameof(repuesto));
            
            // Actualizar la cantidad en el inventario
            repuesto.ActualizarCantidad(repuesto.Id, cantidad);
            
            Console.WriteLine($"Inventario actualizado: {repuesto.Nombre} - Cantidad: {repuesto.CantidadStock}");
        }

        public void RegistrarClientes(Cliente cliente)
        {
            if (cliente == null) throw new ArgumentNullException(nameof(cliente));
            
            // Aquí se registraría el cliente en la base de datos o lista del taller
            Console.WriteLine($"Cliente registrado: {cliente.Nombre} - ID: {cliente.Id}");
        }

        public void RegistrarVehiculo(Vehiculo vehiculo)
        {
            if (vehiculo == null) throw new ArgumentNullException(nameof(vehiculo));
            
            // Aquí se registraría el vehículo en la base de datos o lista del taller
            Console.WriteLine($"Vehículo registrado: {vehiculo.Marca} {vehiculo.Modelo} - Placa: {vehiculo.Placa}");
        }
    }
}
