using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Clases.Vehiculos;

namespace B_TallerAutomoviles.Interfaces
{
    public interface IRegistroEntradaSalidaVehiculo
    {
        void RegistrarEntradaVehiculo(Vehiculo vehiculo);
        void RegistrarSalidaVehiculo(Vehiculo vehiculo);
    }
}
