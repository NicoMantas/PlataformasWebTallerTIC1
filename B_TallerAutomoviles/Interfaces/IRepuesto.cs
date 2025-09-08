using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_TallerAutomoviles.Interfaces
{
    public interface IRepuesto
    {
        void ActualizarCantidad(long id, int cantidad);
    }
}
