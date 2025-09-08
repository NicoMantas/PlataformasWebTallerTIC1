using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Clases;

namespace B_TallerAutomoviles.Interfaces
{
    public interface IGestionInventario
    {
        void GestionarInventario(Repuesto repuesto, int cantidad);
    }
}
