namespace Taller_TIC1_Backend.Models
{
    public class CEmpresa
    {
        public int IdCliente { get; set; }
        public long Nit { get; set; }
        public string RepresentanteLegal { get; set; } = string.Empty;
        
        // Propiedad de navegación
        public Cliente? Cliente { get; set; }


    }
}
