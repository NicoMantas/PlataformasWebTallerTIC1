using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Interfaces;

namespace B_TallerAutomoviles.Clases
{
    public class Repuesto : IRepuesto
    {
        private long id;
        private string nombre;
        private long numeroDeParte;
        private string descripcion;
        private double costoCompra;
        private double costoVenta;
        private int cantidadStock;
        private Proveedor proveedor;

        protected readonly IValidarRepuesto validarRepuesto;

        public long Id { get => id; set => id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public long NumeroDeParte { get => numeroDeParte; set => numeroDeParte = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public double CostoCompra { get => costoCompra; set => costoCompra = value; }
        public double CostoVenta { get => costoVenta; set => costoVenta = value; }
        public int CantidadStock { get => cantidadStock; set => cantidadStock = value; }
        internal Proveedor Proveedor { get => proveedor; set => proveedor = value; }

        public Repuesto(long id, string nombre, long numeroDeParte, string descripcion, double costoCompra, double costoVenta, int cantidadStock, Proveedor proveedor, IValidarRepuesto validarRepuesto)
        {
            this.Id = id;
            this.Nombre = nombre;
            this.NumeroDeParte = numeroDeParte;
            this.Descripcion = descripcion;
            this.CostoCompra = costoCompra;
            this.CostoVenta = costoVenta;
            this.CantidadStock = cantidadStock;
            this.Proveedor = proveedor;
            this.validarRepuesto = validarRepuesto;
            
            // Validar después de asignar los valores
            if (!validarRepuesto.ValidarRepuesto(this))
                throw new ArgumentException("Datos del repuesto inválidos");
        }

        public void ActualizarCantidad(long id, int cantidad)
        {
            if (id <= 0) throw new ArgumentException("ID de repuesto inválido");
            
            // Si la cantidad es negativa, verificar que haya suficiente stock
            if (cantidad < 0 && Math.Abs(cantidad) > this.CantidadStock)
            {
                throw new InvalidOperationException("No hay suficiente stock disponible");
            }
            
            // Actualizar la cantidad
            this.CantidadStock += cantidad;
            
            // Verificar que no quede stock negativo
            if (this.CantidadStock < 0)
            {
                this.CantidadStock = 0;
            }
        }
    }
}
