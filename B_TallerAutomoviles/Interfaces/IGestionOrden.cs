using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Clases.Servicios;

namespace B_TallerAutomoviles.Interfaces
{
    public interface IGestionOrden
    {
        void AgregarServicioOrden(Servicio servicio);
        void AbrirOrden();
        void CerrarOrden();
    }
}
