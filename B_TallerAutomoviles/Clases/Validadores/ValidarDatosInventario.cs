using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_TallerAutomoviles.Interfaces;

namespace B_TallerAutomoviles.Clases.Validadores
{
    public class ValidarDatosInventario : IValidarRepuesto, IValidarProveedor
    {
        public bool ValidarProveedor(Proveedor proveedor)
        {
            if (proveedor == null) return false;
            
            // Validar ID
            if (proveedor.Id <= 0) return false;
            
            // Validar nombre
            if (string.IsNullOrWhiteSpace(proveedor.Nombre) || proveedor.Nombre.Length < 2) return false;
            
            // Validar contacto (debe tener al menos 7 dígitos)
            if (proveedor.Contacto <= 0 || proveedor.Contacto.ToString().Length < 7) return false;
            
            return true;
        }

        public bool ValidarRepuesto(Repuesto repuesto)
        {
            if (repuesto == null) return false;
            
            // Validar ID
            if (repuesto.Id <= 0) return false;
            
            // Validar nombre
            if (string.IsNullOrWhiteSpace(repuesto.Nombre) || repuesto.Nombre.Length < 2) return false;
            
            // Validar número de parte
            if (repuesto.NumeroDeParte <= 0) return false;
            
            // Validar descripción
            if (string.IsNullOrWhiteSpace(repuesto.Descripcion) || repuesto.Descripcion.Length < 5) return false;
            
            // Validar costos
            if (repuesto.CostoCompra <= 0 || repuesto.CostoVenta <= 0) return false;
            
            // Validar que el costo de venta sea mayor al de compra
            if (repuesto.CostoVenta <= repuesto.CostoCompra) return false;
            
            // Validar cantidad en stock
            if (repuesto.CantidadStock < 0) return false;
            
            // Validar proveedor
            if (repuesto.Proveedor == null) return false;
            
            return true;
        }
    }
}
