using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Clases;

namespace B_TallerAutomoviles.Interfaces
{
    public interface IReparacionRepuesto
    {
        void AgregarRepuesto(Repuesto repuesto, int cantidad);
    }
}
