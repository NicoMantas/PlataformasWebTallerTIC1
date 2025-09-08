using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Clases.Vehiculos;
using B_TallerAutomoviles.Interfaces;

namespace B_TallerAutomoviles.Clases.Servicios
{
    public class Revision : Servicio, IRevision
    {
        private string diagnostico;
        private List<string> listaVerificacion;

        public Revision(long id, Vehiculo vehiculo, string descripcion, double costoBase, int tiempoEstimado, Estado estado, IGestionServicio gestionarServicio, string diagnostico) 
            : base(id, vehiculo, descripcion, costoBase, tiempoEstimado, estado, gestionarServicio)
        {
            this.Diagnostico = diagnostico;
            this.ListaVerificacion = new List<string>();
        }

        public string Diagnostico { get => diagnostico; set => diagnostico = value; }
        public List<string> ListaVerificacion { get => listaVerificacion; set => listaVerificacion = value; }

        public void RealizarDiagnostico()
        {
            if (string.IsNullOrWhiteSpace(Diagnostico))
            {
                // Generar un diagnóstico básico basado en la descripción del servicio
                Diagnostico = $"Revisión técnica del vehículo {Vehiculo.Marca} {Vehiculo.Modelo} ({Vehiculo.Placa}). " +
                             $"Servicio: {Descripcion}. " +
                             $"Tiempo estimado: {TiempoEstimado} minutos.";
            }
            
            // Agregar elementos básicos a la lista de verificación si está vacía
            if (ListaVerificacion.Count == 0)
            {
                ListaVerificacion.AddRange(new string[]
                {
                    "Revisar sistema de frenos",
                    "Verificar niveles de fluidos",
                    "Inspeccionar neumáticos",
                    "Revisar sistema eléctrico",
                    "Verificar documentación del vehículo"
                });
            }
            
            // Cambiar el estado a "En proceso" si está pendiente
            if (Estado_enum == Estado.Pendiente)
            {
                Estado_enum = Estado.Enproceso;
            }
        }
    }
}
