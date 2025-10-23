using Microsoft.AspNetCore.Mvc;
using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Services.Interfaces;

namespace Taller_TIC1_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadisticasAdminController : ControllerBase
    {
        private readonly IEstadisticasAdminService _estadisticasService;

        public EstadisticasAdminController(IEstadisticasAdminService estadisticasService)
        {
            _estadisticasService = estadisticasService;
        }

        [HttpGet("completas")]
        public async Task<ActionResult<EstadisticasAdminDto>> GetEstadisticasCompletas()
        {
            try
            {
                var estadisticas = await _estadisticasService.GetEstadisticasCompletasAsync();
                return Ok(estadisticas);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("resumen")]
        public async Task<ActionResult<ResumenGeneralDto>> GetResumenGeneral()
        {
            try
            {
                var resumen = await _estadisticasService.GetResumenGeneralAsync();
                return Ok(resumen);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("servicios")]
        public async Task<ActionResult<EstadisticasServiciosDto>> GetEstadisticasServicios()
        {
            try
            {
                var estadisticas = await _estadisticasService.GetEstadisticasServiciosAsync();
                return Ok(estadisticas);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("empleados")]
        public async Task<ActionResult<EstadisticasEmpleadosDto>> GetEstadisticasEmpleados()
        {
            try
            {
                var estadisticas = await _estadisticasService.GetEstadisticasEmpleadosAsync();
                return Ok(estadisticas);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("facturacion")]
        public async Task<ActionResult<EstadisticasFacturacionDto>> GetEstadisticasFacturacion()
        {
            try
            {
                var estadisticas = await _estadisticasService.GetEstadisticasFacturacionAsync();
                return Ok(estadisticas);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("reportes-mensuales")]
        public async Task<ActionResult<List<ReporteMensualDto>>> GetReportesMensuales([FromQuery] int mesesAtras = 12)
        {
            try
            {
                var reportes = await _estadisticasService.GetReportesMensualesAsync(mesesAtras);
                return Ok(reportes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("capacidad-taller")]
        public async Task<ActionResult<object>> GetCapacidadTallerDetallada()
        {
            try
            {
                var capacidad = await _estadisticasService.GetCapacidadTallerDetalladaAsync();
                return Ok(capacidad);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("dashboard")]
        public async Task<ActionResult<object>> GetDashboardData()
        {
            try
            {
                var resumen = await _estadisticasService.GetResumenGeneralAsync();
                var capacidad = await _estadisticasService.GetCapacidadTallerDetalladaAsync();
                var servicios = await _estadisticasService.GetEstadisticasServiciosAsync();
                var facturacion = await _estadisticasService.GetEstadisticasFacturacionAsync();

                return Ok(new
                {
                    resumen,
                    capacidad,
                    servicios = new
                    {
                        servicios.PorEstado,
                        servicios.TopMecanicos,
                        servicios.ServiciosPorDia
                    },
                    facturacion = new
                    {
                        facturacion.TotalFacturado,
                        facturacion.FacturadoMesActual,
                        facturacion.PorcentajeCrecimiento,
                        facturacion.TopClientes
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("debug-servicios-completados")]
        public async Task<ActionResult<object>> GetDebugServiciosCompletados()
        {
            try
            {
                var serviciosCompletados = await _estadisticasService.GetDebugServiciosCompletadosAsync();
                return Ok(serviciosCompletados);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("debug-todos-servicios")]
        public async Task<ActionResult<object>> GetDebugTodosServicios()
        {
            try
            {
                var todosServicios = await _estadisticasService.GetDebugTodosServiciosAsync();
                return Ok(todosServicios);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("debug-estados-servicios")]
        public async Task<ActionResult<object>> GetDebugEstadosServicios()
        {
            try
            {
                var estados = await _estadisticasService.GetDebugEstadosServiciosAsync();
                return Ok(estados);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("debug-capacidad-taller")]
        public async Task<ActionResult<object>> GetDebugCapacidadTaller()
        {
            try
            {
                var debug = await _estadisticasService.GetDebugCapacidadTallerAsync();
                return Ok(debug);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
