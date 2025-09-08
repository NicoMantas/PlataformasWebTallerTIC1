using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Clases;
using B_TallerAutomoviles.Clases.Empleados;

namespace B_TallerAutomoviles.Interfaces
{
    public interface IGestionTallerMecanico
    {
        void AsignarMecanico(OrdenDeTrabajo ordenDeTrabajo, Mecanico mecanico);
    }
}
