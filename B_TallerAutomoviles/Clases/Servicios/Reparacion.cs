using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Clases.Vehiculos;
using B_TallerAutomoviles.Interfaces;

namespace B_TallerAutomoviles.Clases.Servicios
{
    public class Reparacion : Servicio, IReparacionRepuesto
    {
        private Dictionary<Repuesto, int> repuestosUsados;
        private double manoObra;

        public Reparacion(long id, Vehiculo vehiculo, string descripcion, double costoBase, int tiempoEstimado, Estado estado, IGestionServicio gestionarServicio, double manoObra)
            : base(id, vehiculo, descripcion, costoBase, tiempoEstimado, estado, gestionarServicio)
        {
            this.RepuestosUsados = new Dictionary<Repuesto, int>();
            this.ManoObra = manoObra;
        }

        public double ManoObra { get => manoObra; set => manoObra = value; }
        internal Dictionary<Repuesto, int> RepuestosUsados { get => repuestosUsados; set => repuestosUsados = value; }

        public void AgregarRepuesto(Repuesto repuesto, int cantidad)
        {
            if (repuesto == null) throw new ArgumentNullException(nameof(repuesto));
            if (cantidad <= 0) throw new ArgumentException("La cantidad debe ser mayor a 0");
            
            // Verificar que haya suficiente stock
            if (repuesto.CantidadStock < cantidad)
            {
                throw new InvalidOperationException($"Stock insuficiente. Disponible: {repuesto.CantidadStock}, Solicitado: {cantidad}");
            }
            
            // Si el repuesto ya existe en la lista, sumar la cantidad
            if (RepuestosUsados.ContainsKey(repuesto))
            {
                RepuestosUsados[repuesto] += cantidad;
            }
            else
            {
                RepuestosUsados.Add(repuesto, cantidad);
            }
            
            // Actualizar el stock del repuesto
            repuesto.ActualizarCantidad(repuesto.Id, -cantidad);
        }
    }
}
