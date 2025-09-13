using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_TallerAutomoviles.Clases
{
    public class Repuesto
    {
        private int id;
        private string nombre;
        private long numero_serie;
        private float precio;
        private int stock;
        private List<Proveedor> proveedores;

        public Repuesto(int id, string nombre, long numero_serie, float precio, int stock, List<Proveedor> proveedores)
        {
            this.Id = id;
            this.Nombre = nombre;
            this.Numero_serie = numero_serie;
            this.Precio = precio;
            this.Stock = stock;
            this.Proveedores = proveedores;
        }

        public int Id { get => id; set => id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public long Numero_serie { get => numero_serie; set => numero_serie = value; }
        public float Precio { get => precio; set => precio = value; }
        public int Stock { get => stock; set => stock = value; }
        public List<Proveedor> Proveedores { get => proveedores; set => proveedores = value; }
    }
}
