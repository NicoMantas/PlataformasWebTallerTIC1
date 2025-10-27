using Microsoft.EntityFrameworkCore;
using Taller_TIC1_Backend.Data;
using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Services.Interfaces;

namespace Taller_TIC1_Backend.Services
{
    public class EstadisticasAdminService : IEstadisticasAdminService
    {
        private readonly ApplicationDbContext _context;

        public EstadisticasAdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EstadisticasAdminDto> GetEstadisticasCompletasAsync()
        {
            return new EstadisticasAdminDto
            {
                ResumenGeneral = await GetResumenGeneralAsync(),
                EstadisticasServicios = await GetEstadisticasServiciosAsync(),
                EstadisticasEmpleados = await GetEstadisticasEmpleadosAsync(),
                EstadisticasFacturacion = await GetEstadisticasFacturacionAsync(),
                Tendencias = await GetTendenciasAsync(),
                ReportesMensuales = await GetReportesMensualesAsync()
            };
        }

        public async Task<ResumenGeneralDto> GetResumenGeneralAsync()
        {
            var totalServicios = await _context.Servicios.CountAsync();
            var serviciosActivos = await _context.Servicios.CountAsync(s => s.IdEstado != 3 && s.IdEstado != 5);
            
            // Contar servicios completados con costo > 0
            var serviciosCompletados = await _context.Servicios
                .CountAsync(s => s.IdEstado == 3 && s.Costo >= 0);
            
            var totalClientes = await _context.Clientes.CountAsync(c => c.Activo);
            var totalEmpleados = await _context.Empleados.CountAsync(e => e.Activo);
            var totalVehiculos = await _context.Vehiculos.CountAsync();

            // Calcular ingresos solo de servicios completados con costo
            var serviciosCompletadosConCosto = await _context.Servicios
                .Where(s => s.IdEstado == 3 && s.Costo > 0)
                .ToListAsync();
            
            var ingresosTotales = serviciosCompletadosConCosto.Sum(s => s.Costo);

            var fechaActual = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            var mesActual = fechaActual.Month;
            var añoActual = fechaActual.Year;
            
            // Ingresos del mes actual
            var serviciosMesActual = serviciosCompletadosConCosto
                .Where(s => s.FechaCreacion.Month == mesActual && s.FechaCreacion.Year == añoActual)
                .ToList();
            
            var ingresosMesActual = serviciosMesActual.Sum(s => s.Costo);

            var fechaInicio30 = DateTime.SpecifyKind(DateTime.Now.AddDays(-30), DateTimeKind.Utc);
            var serviciosPorDia = await _context.Servicios
                .Where(s => s.FechaCreacion >= fechaInicio30)
                .CountAsync() / 30.0;

            // Capacidad del taller
            var serviciosPendientes = await _context.Servicios
                .CountAsync(s => s.IdEstado == 1 && s.IdEmpleado == null);
            const int CAPACIDAD_MAXIMA = 15;

            return new ResumenGeneralDto
            {
                TotalServicios = totalServicios,
                ServiciosActivos = serviciosActivos,
                ServiciosCompletados = serviciosCompletados,
                TotalClientes = totalClientes,
                TotalEmpleados = totalEmpleados,
                TotalVehiculos = totalVehiculos,
                IngresosTotales = ingresosTotales,
                IngresosMesActual = ingresosMesActual,
                PromedioServiciosPorDia = Math.Round(serviciosPorDia, 1),
                CapacidadTallerUtilizada = serviciosPendientes,
                CapacidadTallerDisponible = CAPACIDAD_MAXIMA - serviciosPendientes
            };
        }

        public async Task<EstadisticasServiciosDto> GetEstadisticasServiciosAsync()
        {
            var serviciosPorEstado = await _context.Servicios
                .Include(s => s.Estado)
                .GroupBy(s => s.Estado.Descripcion)
                .Select(g => new ServicioPorEstadoDto
                {
                    Estado = g.Key,
                    Cantidad = g.Count(),
                    Porcentaje = 0 // Se calculará después
                })
                .ToListAsync();

            var totalServicios = serviciosPorEstado.Sum(s => s.Cantidad);
            foreach (var servicio in serviciosPorEstado)
            {
                servicio.Porcentaje = totalServicios > 0 ? Math.Round((double)servicio.Cantidad / totalServicios * 100, 1) : 0;
            }

            // Asignar colores a los estados
            foreach (var servicio in serviciosPorEstado)
            {
                servicio.Color = servicio.Estado switch
                {
                    "Pendiente" => "#FFA500",
                    "En proceso" => "#007BFF",
                    "En revisión" => "#6F42C1",
                    "Completado" => "#28A745",
                    "Cancelado" => "#DC3545",
                    _ => "#6C757D"
                };
            }

            var topMecanicos = await _context.Servicios
                .Include(s => s.Empleado)
                .Where(s => s.IdEstado == 4 && s.IdEmpleado != null)
                .GroupBy(s => new { s.IdEmpleado, s.Empleado.Nombre })
                .Select(g => new TopMecanicoDto
                {
                    Nombre = g.Key.Nombre,
                    ServiciosCompletados = g.Count(),
                    IngresosGenerados = g.Sum(s => s.Costo),
                    CalificacionPromedio = 4.5 // Placeholder - implementar sistema de calificaciones si es necesario
                })
                .OrderByDescending(m => m.ServiciosCompletados)
                .Take(5)
                .ToListAsync();

            var fechaInicio = DateTime.SpecifyKind(DateTime.Now.AddDays(-7), DateTimeKind.Utc);
            var serviciosPorDia = await _context.Servicios
                .Where(s => s.FechaCreacion >= fechaInicio)
                .GroupBy(s => s.FechaCreacion.Date)
                .Select(g => new ServicioPorDiaDto
                {
                    Fecha = g.Key,
                    Cantidad = g.Count(),
                    Ingresos = g.Sum(s => s.IdEstado == 4 ? s.Costo : 0)
                })
                .OrderBy(s => s.Fecha)
                .ToListAsync();

            return new EstadisticasServiciosDto
            {
                PorEstado = serviciosPorEstado,
                PorTipo = await GetServiciosPorTipoAsync(),
                TopMecanicos = topMecanicos,
                TiempoPromedioServicio = await CalcularTiempoPromedioServicioAsync(),
                ServiciosPorDia = serviciosPorDia
            };
        }

        public async Task<EstadisticasEmpleadosDto> GetEstadisticasEmpleadosAsync()
        {
            var totalActivos = await _context.Empleados.CountAsync(e => e.Activo);
            var totalInactivos = await _context.Empleados.CountAsync(e => !e.Activo);

            var empleadosPorTipo = await _context.Empleados
                .Include(e => e.TipoEmpleado)
                .Where(e => e.Activo)
                .GroupBy(e => e.TipoEmpleado.Descripcion)
                .Select(g => new EmpleadoPorTipoDto
                {
                    Tipo = g.Key,
                    Cantidad = g.Count(),
                    Porcentaje = 0 // Se calculará después
                })
                .ToListAsync();

            var totalEmpleadosActivos = empleadosPorTipo.Sum(e => e.Cantidad);
            foreach (var empleado in empleadosPorTipo)
            {
                empleado.Porcentaje = totalEmpleadosActivos > 0 ? Math.Round((double)empleado.Cantidad / totalEmpleadosActivos * 100, 1) : 0;
            }

            var promedioSalario = await _context.Empleados
                .Where(e => e.Activo)
                .AverageAsync(e => e.Salario);

            var fechaLimite = DateTime.SpecifyKind(DateTime.Now.AddMonths(-3), DateTimeKind.Utc);
            var empleadosRecientes = await _context.Empleados
                .Include(e => e.TipoEmpleado)
                .Where(e => e.FechaContratacion >= fechaLimite)
                .OrderByDescending(e => e.FechaContratacion)
                .Take(5)
                .Select(e => new EmpleadoRecienteDto
                {
                    Nombre = $"{e.Nombre} {e.Apellido}",
                    Tipo = e.TipoEmpleado.Descripcion,
                    FechaContratacion = e.FechaContratacion,
                    Activo = e.Activo
                })
                .ToListAsync();

            return new EstadisticasEmpleadosDto
            {
                TotalActivos = totalActivos,
                TotalInactivos = totalInactivos,
                PorTipo = empleadosPorTipo,
                PromedioSalario = Math.Round(promedioSalario, 2),
                EmpleadosRecientes = empleadosRecientes
            };
        }

        public async Task<EstadisticasFacturacionDto> GetEstadisticasFacturacionAsync()
        {
            // Obtener todos los servicios completados (estado 3) con sus costos
            var serviciosCompletados = await _context.Servicios
                .Where(s => s.IdEstado == 3 && s.Costo > 0)
                .ToListAsync();

            var totalFacturado = serviciosCompletados.Sum(s => s.Costo);

            var fechaActual = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            var mesActual = fechaActual.Month;
            var añoActual = fechaActual.Year;
            
            // Servicios completados del mes actual
            var serviciosMesActual = serviciosCompletados
                .Where(s => s.FechaCreacion.Month == mesActual && s.FechaCreacion.Year == añoActual)
                .ToList();
            
            var facturadoMesActual = serviciosMesActual.Sum(s => s.Costo);

            // Servicios completados del mes anterior
            var mesAnterior = mesActual == 1 ? 12 : mesActual - 1;
            var añoAnterior = mesActual == 1 ? añoActual - 1 : añoActual;
            
            var serviciosMesAnterior = serviciosCompletados
                .Where(s => s.FechaCreacion.Month == mesAnterior && s.FechaCreacion.Year == añoAnterior)
                .ToList();
            
            var facturadoMesAnterior = serviciosMesAnterior.Sum(s => s.Costo);

            var porcentajeCrecimiento = facturadoMesAnterior > 0 
                ? Math.Round((double)(facturadoMesActual - facturadoMesAnterior) / (double)facturadoMesAnterior * 100, 1)
                : (facturadoMesActual > 0 ? 100.0 : 0);

            var facturacionPorMes = await GetFacturacionPorMesAsync();
            var topClientes = await GetTopClientesAsync();

            return new EstadisticasFacturacionDto
            {
                TotalFacturado = totalFacturado,
                FacturadoMesActual = facturadoMesActual,
                FacturadoMesAnterior = facturadoMesAnterior,
                PorcentajeCrecimiento = porcentajeCrecimiento,
                FacturacionPorMes = facturacionPorMes,
                TopClientes = topClientes
            };
        }

        public async Task<object> GetDebugServiciosCompletadosAsync()
        {
            // Obtener todos los servicios con estado 3
            var serviciosCompletados = await _context.Servicios
                .Where(s => s.IdEstado == 3)
                .Select(s => new
                {
                    s.Id,
                    s.Costo,
                    s.FechaCreacion,
                    s.IdCliente,
                    s.IdVehiculo,
                    s.IdEmpleado,
                    s.IdEstado
                })
                .ToListAsync();

            // Obtener todos los servicios con estado 4 y costo > 0
            var serviciosCompletadosConCosto = serviciosCompletados
                .Where(s => s.Costo > 0)
                .ToList();

            // Calcular totales
            var totalFacturado = serviciosCompletadosConCosto.Sum(s => s.Costo);
            var fechaActual = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            var mesActual = fechaActual.Month;
            var añoActual = fechaActual.Year;

            var serviciosMesActual = serviciosCompletadosConCosto
                .Where(s => s.FechaCreacion.Month == mesActual && s.FechaCreacion.Year == añoActual)
                .ToList();

            var facturadoMesActual = serviciosMesActual.Sum(s => s.Costo);

            return new
            {
                TotalServiciosCompletados = serviciosCompletados.Count,
                ServiciosCompletadosConCosto = serviciosCompletadosConCosto.Count,
                TotalFacturado = totalFacturado,
                FacturadoMesActual = facturadoMesActual,
                ServiciosMesActual = serviciosMesActual.Count,
                ServiciosDetalle = serviciosCompletados.Select(s => new
                {
                    s.Id,
                    s.Costo,
                    s.FechaCreacion,
                    s.IdEstado,
                    TieneCosto = s.Costo > 0
                }).ToList()
            };
        }

        public async Task<object> GetDebugTodosServiciosAsync()
        {
            // Obtener TODOS los servicios con sus estados
            var todosServicios = await _context.Servicios
                .Include(s => s.Estado)
                .Select(s => new
                {
                    s.Id,
                    s.Costo,
                    s.FechaCreacion,
                    s.IdEstado,
                    EstadoDescripcion = s.Estado != null ? s.Estado.Descripcion : "Sin Estado",
                    s.IdCliente,
                    s.IdVehiculo,
                    s.IdEmpleado
                })
                .OrderBy(s => s.Id)
                .ToListAsync();

            // Agrupar por estado
            var serviciosPorEstado = todosServicios
                .GroupBy(s => new { s.IdEstado, s.EstadoDescripcion })
                .Select(g => new
                {
                    IdEstado = g.Key.IdEstado,
                    EstadoDescripcion = g.Key.EstadoDescripcion,
                    Cantidad = g.Count(),
                    Servicios = g.Select(s => new
                    {
                        s.Id,
                        s.Costo,
                        s.FechaCreacion,
                        TieneCosto = s.Costo > 0
                    }).ToList()
                })
                .ToList();

            return new
            {
                TotalServicios = todosServicios.Count,
                ServiciosPorEstado = serviciosPorEstado,
                TodosServicios = todosServicios
            };
        }

        public async Task<object> GetDebugEstadosServiciosAsync()
        {
            // Obtener todos los estados disponibles
            var estadosDisponibles = await _context.EstadosServicio
                .Select(e => new
                {
                    e.Id,
                    e.Descripcion
                })
                .ToListAsync();

            // Obtener todos los servicios con sus estados
            var serviciosConEstados = await _context.Servicios
                .Include(s => s.Estado)
                .Select(s => new
                {
                    s.Id,
                    s.Costo,
                    s.FechaCreacion,
                    s.IdEstado,
                    EstadoDescripcion = s.Estado != null ? s.Estado.Descripcion : "Sin Estado"
                })
                .ToListAsync();

            // Contar servicios por estado
            var conteoPorEstado = serviciosConEstados
                .GroupBy(s => new { s.IdEstado, s.EstadoDescripcion })
                .Select(g => new
                {
                    IdEstado = g.Key.IdEstado,
                    EstadoDescripcion = g.Key.EstadoDescripcion,
                    Cantidad = g.Count(),
                    ServiciosConCosto = g.Count(s => s.Costo > 0),
                    TotalCosto = g.Where(s => s.Costo > 0).Sum(s => s.Costo)
                })
                .OrderBy(x => x.IdEstado)
                .ToList();

            return new
            {
                EstadosDisponibles = estadosDisponibles,
                ConteoPorEstado = conteoPorEstado,
                TotalServicios = serviciosConEstados.Count,
                ServiciosConCosto = serviciosConEstados.Count(s => s.Costo > 0),
                TotalCosto = serviciosConEstados.Where(s => s.Costo > 0).Sum(s => s.Costo)
            };
        }

        public async Task<List<ReporteMensualDto>> GetReportesMensualesAsync(int mesesAtras = 12)
        {
            var fechaInicio = DateTime.SpecifyKind(DateTime.Now.AddMonths(-mesesAtras), DateTimeKind.Utc);
            var reportes = new List<ReporteMensualDto>();

            for (int i = 0; i < mesesAtras; i++)
            {
                var fecha = DateTime.SpecifyKind(DateTime.Now.AddMonths(-i), DateTimeKind.Utc);
                var mes = fecha.Month;
                var año = fecha.Year;

                var serviciosCompletados = await _context.Servicios
                    .CountAsync(s => s.IdEstado == 4 && s.FechaCreacion.Month == mes && s.FechaCreacion.Year == año);

                var ingresos = await _context.Servicios
                    .Where(s => s.IdEstado == 4 && s.FechaCreacion.Month == mes && s.FechaCreacion.Year == año)
                    .SumAsync(s => s.Costo);

                var nuevosClientes = await _context.Clientes
                    .CountAsync(c => c.Id > 0); // Simplificado ya que Cliente no tiene FechaCreacion

                reportes.Add(new ReporteMensualDto
                {
                    Mes = mes,
                    Año = año,
                    NombreMes = fecha.ToString("MMMM"),
                    ServiciosCompletados = serviciosCompletados,
                    Ingresos = ingresos,
                    NuevosClientes = nuevosClientes,
                    SatisfaccionPromedio = 4.2 // Placeholder
                });
            }

            return reportes.OrderBy(r => r.Año).ThenBy(r => r.Mes).ToList();
        }

        public async Task<object> GetCapacidadTallerDetalladaAsync()
        {
            const int CAPACIDAD_MAXIMA = 15;
            
            // Obtener solo los servicios activos (estados: Pendiente, En proceso)
            // NO contar completados (3) ni cancelados (5)
            var serviciosActivos = await _context.Servicios
                .Where(s => s.IdEstado == 1 || s.IdEstado == 2)
                .ToListAsync();
            
            // Separar por tipo para estadísticas detalladas
            var serviciosPendientes = serviciosActivos.Count(s => s.IdEstado == 1);
            var serviciosAsignados = serviciosActivos.Count(s => s.IdEstado == 2);
            
            var totalActivos = serviciosActivos.Count;
            var espaciosDisponibles = CAPACIDAD_MAXIMA - totalActivos;

            return new
            {
                capacidadMaxima = CAPACIDAD_MAXIMA,
                serviciosPendientes,
                serviciosAsignados,
                totalActivos,
                espaciosDisponibles,
                capacidadDisponible = espaciosDisponibles > 0,
                porcentajeOcupacion = Math.Round((double)totalActivos / CAPACIDAD_MAXIMA * 100, 2),
                alerta = totalActivos >= CAPACIDAD_MAXIMA ? "Capacidad al límite" : espaciosDisponibles <= 3 ? "Capacidad casi llena" : "Capacidad normal"
            };
        }

        public async Task<object> GetDebugCapacidadTallerAsync()
        {
            const int CAPACIDAD_MAXIMA = 15;
            
            // Obtener todos los servicios con sus detalles
            var todosServicios = await _context.Servicios
                .Include(s => s.Estado)
                .Include(s => s.Empleado)
                .Select(s => new
                {
                    s.Id,
                    s.IdEstado,
                    EstadoDescripcion = s.Estado != null ? s.Estado.Descripcion : "Sin Estado",
                    s.IdEmpleado,
                    EmpleadoNombre = s.Empleado != null ? s.Empleado.Nombre : null,
                    s.FechaCreacion,
                    s.FechaActualizacion
                })
                .ToListAsync();

            // Filtrar servicios activos (solo estados 1 y 2)
            var serviciosActivos = todosServicios.Where(s => s.IdEstado == 1 || s.IdEstado == 2).ToList();
            
            // Separar por tipo
            var serviciosPendientes = serviciosActivos.Where(s => s.IdEstado == 1).ToList();
            var serviciosAsignados = serviciosActivos.Where(s => s.IdEstado == 2).ToList();
            
            var totalActivos = serviciosActivos.Count;
            var espaciosDisponibles = CAPACIDAD_MAXIMA - totalActivos;

            return new
            {
                capacidadMaxima = CAPACIDAD_MAXIMA,
                totalServicios = todosServicios.Count,
                serviciosActivos = serviciosActivos.Count,
                serviciosPendientes = serviciosPendientes.Count,
                serviciosAsignados = serviciosAsignados.Count,
                totalActivos,
                espaciosDisponibles,
                porcentajeOcupacion = Math.Round((double)totalActivos / CAPACIDAD_MAXIMA * 100, 2),
                
                // Detalles para debug
                detalleServicios = new
                {
                    todosServicios = todosServicios.Select(s => new
                    {
                        s.Id,
                        s.IdEstado,
                        s.EstadoDescripcion,
                        s.IdEmpleado,
                        s.EmpleadoNombre,
                        s.FechaCreacion
                    }).ToList(),
                    
                    serviciosPendientes = serviciosPendientes.Select(s => new
                    {
                        s.Id,
                        s.IdEstado,
                        s.EstadoDescripcion,
                        s.IdEmpleado,
                        s.EmpleadoNombre
                    }).ToList(),
                    
                    serviciosAsignados = serviciosAsignados.Select(s => new
                    {
                        s.Id,
                        s.IdEstado,
                        s.EstadoDescripcion,
                        s.IdEmpleado,
                        s.EmpleadoNombre
                    }).ToList(),
                    
                }
            };
        }

        private async Task<List<ServicioPorTipoDto>> GetServiciosPorTipoAsync()
        {
            // Esta implementación es simplificada ya que necesitaríamos más información sobre los tipos de servicio
            var revisiones = await _context.DetallesRevision.CountAsync();
            var reparaciones = await _context.DetallesReparacion.CountAsync();

            var total = revisiones + reparaciones;

            return new List<ServicioPorTipoDto>
            {
                new ServicioPorTipoDto
                {
                    Tipo = "Revisión",
                    Cantidad = revisiones,
                    Porcentaje = total > 0 ? Math.Round((double)revisiones / total * 100, 1) : 0,
                    Ingresos = revisiones * 50000 // Estimación
                },
                new ServicioPorTipoDto
                {
                    Tipo = "Reparación",
                    Cantidad = reparaciones,
                    Porcentaje = total > 0 ? Math.Round((double)reparaciones / total * 100, 1) : 0,
                    Ingresos = reparaciones * 150000 // Estimación
                }
            };
        }

        private async Task<double> CalcularTiempoPromedioServicioAsync()
        {
            var serviciosCompletados = await _context.Servicios
                .Where(s => s.IdEstado == 4 && s.FechaActualizacion != null)
                .Select(s => new { s.FechaCreacion, s.FechaActualizacion })
                .ToListAsync();

            if (!serviciosCompletados.Any()) return 0;

            var tiempoTotal = serviciosCompletados
                .Sum(s => (s.FechaActualizacion.Value - s.FechaCreacion).TotalHours);

            return Math.Round(tiempoTotal / serviciosCompletados.Count, 1);
        }

        private async Task<List<FacturacionPorMesDto>> GetFacturacionPorMesAsync()
        {
            var ultimos12Meses = DateTime.SpecifyKind(DateTime.Now.AddMonths(-11), DateTimeKind.Utc);
            
            return await _context.Servicios
                .Where(s => s.IdEstado == 4 && s.FechaCreacion >= ultimos12Meses)
                .GroupBy(s => new { s.FechaCreacion.Month, s.FechaCreacion.Year })
                .Select(g => new FacturacionPorMesDto
                {
                    Mes = g.Key.Month,
                    Año = g.Key.Year,
                    Monto = g.Sum(s => s.Costo),
                    CantidadFacturas = g.Count()
                })
                .OrderBy(f => f.Año)
                .ThenBy(f => f.Mes)
                .ToListAsync();
        }

        private async Task<List<TopClienteDto>> GetTopClientesAsync()
        {
            return await _context.Servicios
                .Include(s => s.Cliente)
                .Where(s => s.IdEstado == 4)
                .GroupBy(s => new { s.IdCliente, s.Cliente.Nombre })
                .Select(g => new TopClienteDto
                {
                    Nombre = g.Key.Nombre,
                    ServiciosRealizados = g.Count(),
                    TotalGastado = g.Sum(s => s.Costo),
                    UltimoServicio = g.Max(s => s.FechaCreacion)
                })
                .OrderByDescending(c => c.TotalGastado)
                .Take(5)
                .ToListAsync();
        }

        private async Task<EstadisticasTendenciasDto> GetTendenciasAsync()
        {
            var fechaActual = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            var mesActual = fechaActual.Month;
            var añoActual = fechaActual.Year;
            var mesAnterior = mesActual == 1 ? 12 : mesActual - 1;
            var añoAnterior = mesActual == 1 ? añoActual - 1 : añoActual;

            // Servicios
            var serviciosMesActual = await _context.Servicios
                .CountAsync(s => s.FechaCreacion.Month == mesActual && s.FechaCreacion.Year == añoActual);
            var serviciosMesAnterior = await _context.Servicios
                .CountAsync(s => s.FechaCreacion.Month == mesAnterior && s.FechaCreacion.Year == añoAnterior);

            // Ingresos
            var ingresosMesActual = await _context.Servicios
                .Where(s => s.IdEstado == 4 && s.FechaCreacion.Month == mesActual && s.FechaCreacion.Year == añoActual)
                .SumAsync(s => s.Costo);
            var ingresosMesAnterior = await _context.Servicios
                .Where(s => s.IdEstado == 4 && s.FechaCreacion.Month == mesAnterior && s.FechaCreacion.Year == añoAnterior)
                .SumAsync(s => s.Costo);

            // Clientes - Simplificado ya que Cliente no tiene FechaCreacion
            var clientesMesActual = await _context.Clientes.CountAsync(c => c.Activo);
            var clientesMesAnterior = await _context.Clientes.CountAsync(c => c.Activo);

            return new EstadisticasTendenciasDto
            {
                CrecimientoServicios = serviciosMesAnterior > 0 ? Math.Round((double)(serviciosMesActual - serviciosMesAnterior) / serviciosMesAnterior * 100, 1) : 0,
                CrecimientoClientes = clientesMesAnterior > 0 ? Math.Round((double)(clientesMesActual - clientesMesAnterior) / clientesMesAnterior * 100, 1) : 0,
                CrecimientoIngresos = ingresosMesAnterior > 0 ? Math.Round((double)(ingresosMesActual - ingresosMesAnterior) / (double)ingresosMesAnterior * 100, 1) : 0,
                TendenciaServicios = serviciosMesActual > serviciosMesAnterior ? "Crecimiento" : serviciosMesActual < serviciosMesAnterior ? "Descenso" : "Estable",
                TendenciaIngresos = ingresosMesActual > ingresosMesAnterior ? "Crecimiento" : ingresosMesActual < ingresosMesAnterior ? "Descenso" : "Estable"
            };
        }
    }
}
