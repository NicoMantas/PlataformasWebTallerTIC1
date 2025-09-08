using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Clases.Clientes;
using B_TallerAutomoviles.Clases.Servicios;
using B_TallerAutomoviles.Clases.Vehiculos;
using B_TallerAutomoviles.Interfaces;

namespace B_TallerAutomoviles.Clases.Gestion
{
    public class GestionOrden : IGestionOrden, IGestionOrdenTrabajo
    {
        private static long contadorOrdenes = 1;
        
        public void AbrirOrden()
        {
            // Este método podría ser usado para cambiar el estado de una orden a "En Proceso"
            // La implementación específica dependería del contexto
            Console.WriteLine("Orden abierta y lista para trabajo");
        }

        public void AgregarServicioOrden(Servicio servicio)
        {
            if (servicio == null) throw new ArgumentNullException(nameof(servicio));
            
            // Aquí se agregaría el servicio a la orden actual
            // La implementación específica dependería de cómo se maneje la orden actual
            Console.WriteLine($"Servicio '{servicio.Descripcion}' agregado a la orden");
        }

        public void CerrarOrden()
        {
            // Este método cambiaría el estado de la orden a "Completada"
            Console.WriteLine("Orden cerrada y completada");
        }

        public OrdenDeTrabajo CrearOrdenTrabajo(Cliente cliente, Vehiculo vehiculo)
        {
            if (cliente == null) throw new ArgumentNullException(nameof(cliente));
            if (vehiculo == null) throw new ArgumentNullException(nameof(vehiculo));
            
            // Crear una nueva orden de trabajo
            var orden = new OrdenDeTrabajo(
                contadorOrdenes++,
                DateTime.Now,
                OrdenDeTrabajo.estadoOrden.EnProceso,
                cliente,
                vehiculo,
                null // IGestionOrden se pasaría aquí si fuera necesario
            );
            
            Console.WriteLine($"Orden de trabajo #{orden.Id} creada para {cliente.Nombre} - Vehículo: {vehiculo.Marca} {vehiculo.Modelo}");
            
            return orden;
        }
    }
}
