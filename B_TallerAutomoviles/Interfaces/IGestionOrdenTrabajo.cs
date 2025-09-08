using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Clases;
using B_TallerAutomoviles.Clases.Clientes;
using B_TallerAutomoviles.Clases.Vehiculos;

namespace B_TallerAutomoviles.Interfaces
{
    public interface IGestionOrdenTrabajo
    {
        OrdenDeTrabajo CrearOrdenTrabajo(Cliente cliente, Vehiculo vehiculo);
    }
}
