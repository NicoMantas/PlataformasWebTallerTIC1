namespace Taller_TIC1_Backend.Models.DTOs
{
    public class EstadisticasAdminDto
    {
        public ResumenGeneralDto ResumenGeneral { get; set; } = new ResumenGeneralDto();
        public EstadisticasServiciosDto EstadisticasServicios { get; set; } = new EstadisticasServiciosDto();
        public EstadisticasEmpleadosDto EstadisticasEmpleados { get; set; } = new EstadisticasEmpleadosDto();
        public EstadisticasFacturacionDto EstadisticasFacturacion { get; set; } = new EstadisticasFacturacionDto();
        public EstadisticasTendenciasDto Tendencias { get; set; } = new EstadisticasTendenciasDto();
        public List<ReporteMensualDto> ReportesMensuales { get; set; } = new List<ReporteMensualDto>();
    }

    public class ResumenGeneralDto
    {
        public int TotalServicios { get; set; }
        public int ServiciosActivos { get; set; }
        public int ServiciosCompletados { get; set; }
        public int TotalClientes { get; set; }
        public int TotalEmpleados { get; set; }
        public int TotalVehiculos { get; set; }
        public decimal IngresosTotales { get; set; }
        public decimal IngresosMesActual { get; set; }
        public double PromedioServiciosPorDia { get; set; }
        public int CapacidadTallerUtilizada { get; set; }
        public int CapacidadTallerDisponible { get; set; }
    }

    public class EstadisticasServiciosDto
    {
        public List<ServicioPorEstadoDto> PorEstado { get; set; } = new List<ServicioPorEstadoDto>();
        public List<ServicioPorTipoDto> PorTipo { get; set; } = new List<ServicioPorTipoDto>();
        public List<TopMecanicoDto> TopMecanicos { get; set; } = new List<TopMecanicoDto>();
        public double TiempoPromedioServicio { get; set; }
        public List<ServicioPorDiaDto> ServiciosPorDia { get; set; } = new List<ServicioPorDiaDto>();
    }

    public class EstadisticasEmpleadosDto
    {
        public int TotalActivos { get; set; }
        public int TotalInactivos { get; set; }
        public List<EmpleadoPorTipoDto> PorTipo { get; set; } = new List<EmpleadoPorTipoDto>();
        public double PromedioSalario { get; set; }
        public List<EmpleadoRecienteDto> EmpleadosRecientes { get; set; } = new List<EmpleadoRecienteDto>();
    }

    public class EstadisticasFacturacionDto
    {
        public decimal TotalFacturado { get; set; }
        public decimal FacturadoMesActual { get; set; }
        public decimal FacturadoMesAnterior { get; set; }
        public double PorcentajeCrecimiento { get; set; }
        public List<FacturacionPorMesDto> FacturacionPorMes { get; set; } = new List<FacturacionPorMesDto>();
        public List<TopClienteDto> TopClientes { get; set; } = new List<TopClienteDto>();
    }

    public class EstadisticasTendenciasDto
    {
        public double CrecimientoServicios { get; set; }
        public double CrecimientoClientes { get; set; }
        public double CrecimientoIngresos { get; set; }
        public string TendenciaServicios { get; set; } = string.Empty;
        public string TendenciaIngresos { get; set; } = string.Empty;
    }

    public class ServicioPorEstadoDto
    {
        public string Estado { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public double Porcentaje { get; set; }
        public string Color { get; set; } = string.Empty;
    }

    public class ServicioPorTipoDto
    {
        public string Tipo { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public double Porcentaje { get; set; }
        public decimal Ingresos { get; set; }
    }

    public class TopMecanicoDto
    {
        public string Nombre { get; set; } = string.Empty;
        public int ServiciosCompletados { get; set; }
        public double CalificacionPromedio { get; set; }
        public decimal IngresosGenerados { get; set; }
    }

    public class ServicioPorDiaDto
    {
        public DateTime Fecha { get; set; }
        public int Cantidad { get; set; }
        public decimal Ingresos { get; set; }
    }

    public class EmpleadoPorTipoDto
    {
        public string Tipo { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public double Porcentaje { get; set; }
    }

    public class EmpleadoRecienteDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public DateTime FechaContratacion { get; set; }
        public bool Activo { get; set; }
    }

    public class FacturacionPorMesDto
    {
        public int Mes { get; set; }
        public int Año { get; set; }
        public decimal Monto { get; set; }
        public int CantidadFacturas { get; set; }
    }

    public class TopClienteDto
    {
        public string Nombre { get; set; } = string.Empty;
        public int ServiciosRealizados { get; set; }
        public decimal TotalGastado { get; set; }
        public DateTime UltimoServicio { get; set; }
    }

    public class ReporteMensualDto
    {
        public int Mes { get; set; }
        public int Año { get; set; }
        public string NombreMes { get; set; } = string.Empty;
        public int ServiciosCompletados { get; set; }
        public decimal Ingresos { get; set; }
        public int NuevosClientes { get; set; }
        public double SatisfaccionPromedio { get; set; }
    }
}
