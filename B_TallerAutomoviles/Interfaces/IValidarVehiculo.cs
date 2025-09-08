using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Clases.Vehiculos;

namespace B_TallerAutomoviles.Interfaces
{
    public interface IValidarVehiculo
    {
        bool ValidarVehiculo(Vehiculo vehiculo);
    }
}
