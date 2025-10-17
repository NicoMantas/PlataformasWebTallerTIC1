using AutoMapper;
using Taller_TIC1_Backend.Models;
using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Repositories.Interfaces;
using Taller_TIC1_Backend.Services.Interfaces;

namespace Taller_TIC1_Backend.Services
{
    public class ServicioService : IServicioService
    {
        private readonly IServicioRepository _servicioRepository;
        private readonly IMapper _mapper;
        public ServicioService(IServicioRepository servicioRepository, IMapper mapper)
        {
            _servicioRepository = servicioRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ServicioDTO>> GetAllServiciosAsync()
        {
            var servicios = await _servicioRepository.GetAllAsync();
            var serviciosDto = new List<ServicioDTO>();

            foreach (var servicio in servicios)
            {
                var servicioDto = _mapper.Map<ServicioDTO>(servicio);
                
                // Cargar información adicional
                servicioDto.EstadoDescripcion = servicio.Estado?.Descripcion ?? "Sin estado";
                servicioDto.ClienteNombre = servicio.Cliente?.Nombre ?? "Sin cliente";
                servicioDto.EmpleadoNombre = servicio.Empleado?.Nombre ?? "Sin empleado";

                // Determinar tipo de servicio y cargar detalles específicos
                var detalleRevision = await _servicioRepository.GetDetalleRevisionByServicioIdAsync(servicio.Id);
                if (detalleRevision != null)
                {
                    servicioDto.TipoServicio = "Revision";
                    servicioDto.DetallesRevision = detalleRevision.Detalles;
                }
                else
                {
                    var detalleReparacion = await _servicioRepository.GetDetalleReparacionByServicioIdAsync(servicio.Id);
                    if (detalleReparacion != null)
                    {
                        servicioDto.TipoServicio = "Reparacion";
                        var repuestos = await _servicioRepository.GetRepuestosByDetalleReparacionAsync(detalleReparacion.IdDetalleReparacionRepuesto);
                        servicioDto.RepuestosReparacion = _mapper.Map<List<RepuestoCantidadDTO>>(repuestos);
                    }
                }

                serviciosDto.Add(servicioDto);
            }

            return serviciosDto;
        }

        public async Task<ServicioDTO?> GetServicioByIdAsync(int id)
        {
            var servicio = await _servicioRepository.GetByIdAsync(id);
            if (servicio == null) return null;

            var servicioDto = _mapper.Map<ServicioDTO>(servicio);

            // Cargar detalles específicos
            var detalleRevision = await _servicioRepository.GetDetalleRevisionByServicioIdAsync(id);
            if (detalleRevision != null)
            {
                servicioDto.TipoServicio = "Revision";
                servicioDto.DetallesRevision = detalleRevision.Detalles;
            }
            else
            {
                var detalleReparacion = await _servicioRepository.GetDetalleReparacionByServicioIdAsync(id);
                if (detalleReparacion != null)
                {
                    servicioDto.TipoServicio = "Reparacion";
                    var repuestos = await _servicioRepository.GetRepuestosByDetalleReparacionAsync(detalleReparacion.IdDetalleReparacionRepuesto);
                    servicioDto.RepuestosReparacion = _mapper.Map<List<RepuestoCantidadDTO>>(repuestos);
                }
            }

            return servicioDto;
        }

        public async Task<ServicioDTO> CreateServicioAsync(ServicioCreateDTO servicioCreateDto)
        {
            // Validar capacidad del taller antes de crear el servicio
            await ValidarCapacidadTallerAsync();

            var servicio = _mapper.Map<Servicio>(servicioCreateDto);
            var servicioCreado = await _servicioRepository.CreateAsync(servicio);

            // Crear detalles específicos según el tipo de servicio
            if (servicioCreateDto.TipoServicio == "Revision" && !string.IsNullOrEmpty(servicioCreateDto.DetallesRevision))
            {
                var detalleRevisionNuevo = new DetalleRevision
                {
                    IdServicio = servicioCreado.Id,
                    Detalles = servicioCreateDto.DetallesRevision
                };
                await _servicioRepository.CreateDetalleRevisionAsync(detalleRevisionNuevo);
            }
            else if (servicioCreateDto.TipoServicio == "Reparacion" && servicioCreateDto.RepuestosReparacion != null && servicioCreateDto.RepuestosReparacion.Count > 0)
            {
                // Si ya existen repuestos, crear el contenedor y asociarlo
                // NOTA: si se requiere guardar cada repuesto, aquí se debería iterar y persistirlos
                var detalleReparacionRepuesto = new DetalleReparacionRepuesto();
                await _servicioRepository.CreateDetalleReparacionRepuestoAsync(detalleReparacionRepuesto);

                var detalleReparacion = new DetalleReparacion
                {
                    IdServicio = servicioCreado.Id,
                    IdDetalleReparacionRepuesto = detalleReparacionRepuesto.Id
                };
                await _servicioRepository.CreateDetalleReparacionAsync(detalleReparacion);
            }

            // Recargar el servicio con todas las relaciones incluidas para obtener los datos completos
            var servicioCompleto = await _servicioRepository.GetByIdAsync(servicioCreado.Id);
            if (servicioCompleto == null)
                throw new InvalidOperationException("Error al recargar el servicio creado");

            var servicioDto = _mapper.Map<ServicioDTO>(servicioCompleto);

            // Cargar detalles específicos
            var detalleRevision = await _servicioRepository.GetDetalleRevisionByServicioIdAsync(servicioCreado.Id);
            if (detalleRevision != null)
            {
                servicioDto.TipoServicio = "Revision";
                servicioDto.DetallesRevision = detalleRevision.Detalles;
            }
            else
            {
                var detalleReparacion = await _servicioRepository.GetDetalleReparacionByServicioIdAsync(servicioCreado.Id);
                if (detalleReparacion != null)
                {
                    servicioDto.TipoServicio = "Reparacion";
                    var repuestos = await _servicioRepository.GetRepuestosByDetalleReparacionAsync(detalleReparacion.IdDetalleReparacionRepuesto);
                    servicioDto.RepuestosReparacion = _mapper.Map<List<RepuestoCantidadDTO>>(repuestos);
                }
            }

            return servicioDto;
        }

        public async Task<ServicioDTO> UpdateServicioAsync(int id, ServicioDTO servicioDto)
        {
            var servicioExistente = await _servicioRepository.GetByIdAsync(id);
            if (servicioExistente == null)
                throw new ArgumentException("Servicio no encontrado");

            _mapper.Map(servicioDto, servicioExistente);
            var servicioActualizado = await _servicioRepository.UpdateAsync(servicioExistente);
            return _mapper.Map<ServicioDTO>(servicioActualizado);
        }

        public async Task<bool> DeleteServicioAsync(int id)
        {
            return await _servicioRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ServicioDTO>> GetServiciosByClienteAsync(int clienteId)
        {
            var servicios = await _servicioRepository.GetByClienteAsync(clienteId);
            return await MapDetallesServicios(servicios);
        }

        public async Task<IEnumerable<ServicioDTO>> GetServiciosActivosByClienteAsync(int clienteId)
        {
            // Activos: Pendiente (1), En proceso (2)
            var ids = new List<int> { 1, 2 };
            var servicios = await _servicioRepository.GetByClienteAndEstadosAsync(clienteId, ids);
            return await MapDetallesServicios(servicios);
        }

        public async Task<IEnumerable<ServicioDTO>> GetServiciosHistorialByClienteAsync(int clienteId)
        {
            // Historial: solo Completado (IdEstado == 3)
            var ids = new List<int> { 3 };
            var servicios = await _servicioRepository.GetByClienteAndEstadosAsync(clienteId, ids);
            return await MapDetallesServicios(servicios);
        }

        public async Task<bool> CancelarServicioAsync(int id)
        {
            var servicio = await _servicioRepository.GetByIdAsync(id);
            if (servicio == null) return false;
            // IdEstado == 4: Cancelado
            servicio.IdEstado = 4;
            await _servicioRepository.UpdateAsync(servicio);
            return true;
        }

        private async Task<IEnumerable<ServicioDTO>> MapDetallesServicios(IEnumerable<Servicio> servicios)
        {
            var result = new List<ServicioDTO>();
            foreach (var servicio in servicios)
            {
                var dto = _mapper.Map<ServicioDTO>(servicio);
                dto.EstadoDescripcion = servicio.Estado?.Descripcion ?? "";
                dto.ClienteNombre = servicio.Cliente?.Nombre ?? "";
                dto.EmpleadoNombre = servicio.Empleado?.Nombre ?? "";
                dto.IdVehiculo = servicio.IdVehiculo;
                dto.VehiculoPlaca = servicio.Vehiculo?.Placa ?? "";
                dto.VehiculoMarca = servicio.Vehiculo?.Marca ?? "";
                dto.VehiculoModelo = servicio.Vehiculo?.Modelo ?? "";
                dto.VehiculoInfo = $"{servicio.Vehiculo?.Marca} {servicio.Vehiculo?.Modelo} - {servicio.Vehiculo?.Placa}";
                dto.FechaCreacion = servicio.FechaCreacion;
                dto.FechaActualizacion = servicio.FechaActualizacion;

                var detalleRevision = await _servicioRepository.GetDetalleRevisionByServicioIdAsync(servicio.Id);
                if (detalleRevision != null)
                {
                    dto.TipoServicio = "Revision";
                    dto.DetallesRevision = detalleRevision.Detalles;
                    dto.DetallesEncontrados = detalleRevision.DetallesEncontrados;
                }
                else
                {
                    var detalleReparacion = await _servicioRepository.GetDetalleReparacionByServicioIdAsync(servicio.Id);
                    if (detalleReparacion != null)
                    {
                        dto.TipoServicio = "Reparacion";
                        var repuestos = await _servicioRepository.GetRepuestosByDetalleReparacionAsync(detalleReparacion.IdDetalleReparacionRepuesto);
                        dto.RepuestosReparacion = _mapper.Map<List<RepuestoCantidadDTO>>(repuestos);
                    }
                }
                result.Add(dto);
            }
            return result;
        }

        public async Task<IEnumerable<ServicioDTO>> SecretariaListPendientesAsync()
        {
            // Pendiente (1), En proceso (2)
            var ids = new List<int> { 1, 2 };
            var servicios = await _servicioRepository.GetActivosPendientesAsync(ids);
            return await MapDetallesServicios(servicios);
        }

        public async Task<IEnumerable<ServicioDTO>> SecretariaListAsignadosAsync()
        {
            // Pendiente (1), En proceso (2)
            var ids = new List<int> { 1, 2 };
            var servicios = await _servicioRepository.GetActivosAsignadosAsync(ids);
            return await MapDetallesServicios(servicios);
        }

        public async Task<IEnumerable<ServicioDTO>> SecretariaListCompletadosAsync()
        {
            // Completado (IdEstado == 3)
            var ids = new List<int> { 3 };
            var servicios = await _servicioRepository.GetServiciosByEstadosAsync(ids);
            var serviciosDto = await MapDetallesServicios(servicios);

            // Calcular costo total para reparaciones (costo base + repuestos)
            foreach (var servicioDto in serviciosDto)
            {
                if (servicioDto.TipoServicio == "Reparacion")
                {
                    servicioDto.Costo = await CalcularCostoTotalReparacionAsync(servicioDto.Id, servicioDto.Costo);
                }
            }

            return serviciosDto;
        }

        public async Task<bool> SecretariaAsignarMecanicoAsync(int servicioId, int empleadoId)
        {
            return await _servicioRepository.AssignEmpleadoAsync(servicioId, empleadoId);
        }

        public async Task<IEnumerable<ServicioDTO>> GetServiciosByMecanicoAsync(int empleadoId)
        {
            // Servicios asignados y en proceso (no completados)
            // IdEstado == 1: Pendiente, IdEstado == 2: En proceso
            var ids = new List<int> { 1, 2 };
            var servicios = await _servicioRepository.GetServiciosByEmpleadoAndEstadosAsync(empleadoId, ids);
            return await MapDetallesServicios(servicios);
        }

        public async Task<IEnumerable<ServicioDTO>> GetServiciosCompletadosByMecanicoAsync(int empleadoId)
        {
            // Servicios completados por el mecánico (IdEstado == 3)
            var ids = new List<int> { 3 }; // Estado "Completado" es IdEstado == 3
            var servicios = await _servicioRepository.GetServiciosByEmpleadoAndEstadosAsync(empleadoId, ids);
            return await MapDetallesServicios(servicios);
        }

        public async Task<bool> UpdateServicioEstadoAsync(int servicioId, int estadoId)
        {
            return await _servicioRepository.UpdateServicioEstadoAsync(servicioId, estadoId);
        }

        public async Task<bool> DesasignarEmpleadoAsync(int servicioId)
        {
            return await _servicioRepository.DesasignarEmpleadoAsync(servicioId);
        }

        public async Task<IEnumerable<ServicioDTO>> GetHistorialByVehiculoAsync(int vehiculoId)
        {
            var servicios = await _servicioRepository.GetByVehiculoAsync(vehiculoId);
            return await MapDetallesServicios(servicios);
        }

        public async Task<decimal> CalcularCostoTotalReparacionAsync(int servicioId, decimal costoBase)
        {
            var detalleReparacion = await _servicioRepository.GetDetalleReparacionByServicioIdAsync(servicioId);
            if (detalleReparacion == null) return costoBase;

            // El costo del servicio ya incluye los repuestos, no necesitamos sumarlos otra vez
            return costoBase;
        }

        public async Task<object> GetCapacidadTallerAsync()
        {
            const int CAPACIDAD_MAXIMA_TALLER = 15;
            
            // Obtener servicios pendientes (sin mecánico asignado)
            var serviciosPendientes = await _servicioRepository.GetActivosPendientesAsync(new[] { 1 }); // Estado "Pendiente"
            var serviciosAsignados = await _servicioRepository.GetActivosAsignadosAsync(new[] { 2 }); // Estado "En proceso"
            
            var totalPendientes = serviciosPendientes.Count();
            var totalAsignados = serviciosAsignados.Count();
            var totalActivos = totalPendientes + totalAsignados;
            var espaciosDisponibles = CAPACIDAD_MAXIMA_TALLER - totalPendientes;
            
            return new
            {
                capacidadMaxima = CAPACIDAD_MAXIMA_TALLER,
                serviciosPendientes = totalPendientes,
                serviciosAsignados = totalAsignados,
                totalActivos = totalActivos,
                espaciosDisponibles = espaciosDisponibles,
                capacidadDisponible = espaciosDisponibles > 0,
                porcentajeOcupacion = Math.Round((double)totalPendientes / CAPACIDAD_MAXIMA_TALLER * 100, 2)
            };
        }

        public async Task<IEnumerable<ServicioDTO>> GetServiciosByPlacaAsync(string placa)
        {
            var servicios = await _servicioRepository.GetServiciosByPlacaAsync(placa);
            return await MapDetallesServicios(servicios);
        }

        public async Task<IEnumerable<ServicioDTO>> GetServiciosByFechaAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            var servicios = await _servicioRepository.GetServiciosByFechaAsync(fechaInicio, fechaFin);
            return await MapDetallesServicios(servicios);
        }

        public async Task<IEnumerable<ServicioDTO>> GetServiciosByPlacaAndFechaAsync(string placa, DateTime fechaInicio, DateTime fechaFin)
        {
            var servicios = await _servicioRepository.GetServiciosByPlacaAndFechaAsync(placa, fechaInicio, fechaFin);
            return await MapDetallesServicios(servicios);
        }

        public async Task<ServicioProgresoDto?> GetProgresoServicioAsync(int servicioId)
        {
            var servicio = await _servicioRepository.GetByIdAsync(servicioId);
            if (servicio == null) return null;

            var progreso = new ServicioProgresoDto
            {
                Id = servicio.Id,
                EstadoActual = servicio.Estado?.Descripcion ?? "Sin estado",
                IdEstado = servicio.IdEstado,
                ClienteNombre = servicio.Cliente?.Nombre ?? "Sin cliente",
                VehiculoPlaca = servicio.Vehiculo?.Placa ?? "Sin placa",
                VehiculoMarca = servicio.Vehiculo?.Marca ?? "Sin marca",
                VehiculoModelo = servicio.Vehiculo?.Modelo ?? "Sin modelo",
                MecanicoNombre = servicio.Empleado?.Nombre ?? null,
                FechaCreacion = servicio.FechaCreacion,
                FechaActualizacion = servicio.FechaActualizacion,
                Costo = servicio.Costo
            };

            // Determinar tipo de servicio
            var detalleRevision = await _servicioRepository.GetDetalleRevisionByServicioIdAsync(servicioId);
            if (detalleRevision != null)
            {
                progreso.TipoServicio = "Revision";
            }
            else
            {
                var detalleReparacion = await _servicioRepository.GetDetalleReparacionByServicioIdAsync(servicioId);
                if (detalleReparacion != null)
                {
                    progreso.TipoServicio = "Reparacion";
                }
            }

            // Crear flujo de estados
            progreso.FlujoEstados = await CrearFlujoEstadosAsync(servicioId, servicio.IdEstado);
            progreso.PorcentajeCompletado = CalcularPorcentajeCompletado(servicio.IdEstado);
            progreso.MensajeEstado = GenerarMensajeEstado(servicio.IdEstado, progreso.MecanicoNombre);

            return progreso;
        }

        private async Task<List<EstadoProgresoDto>> CrearFlujoEstadosAsync(int servicioId, int estadoActualId)
        {
            var flujoEstados = new List<EstadoProgresoDto>();
            var servicio = await _servicioRepository.GetByIdAsync(servicioId);

            // Definir el flujo de estados
            var estadosFlujo = new[]
            {
                new { Id = 1, Descripcion = "Pendiente", Icono = "⏳", Mensaje = "Servicio creado y esperando asignación de mecánico" },
                new { Id = 2, Descripcion = "En proceso", Icono = "🔧", Mensaje = "Mecánico asignado, trabajo en progreso" },
                new { Id = 3, Descripcion = "En revisión", Icono = "🔍", Mensaje = "Servicio completado, en proceso de revisión final" },
                new { Id = 4, Descripcion = "Completado", Icono = "✅", Mensaje = "Servicio completado y listo para entrega" },
                new { Id = 5, Descripcion = "Cancelado", Icono = "❌", Mensaje = "Servicio cancelado" }
            };

            foreach (var estado in estadosFlujo)
            {
                var estadoProgreso = new EstadoProgresoDto
                {
                    Id = estado.Id,
                    Descripcion = estado.Descripcion,
                    Icono = estado.Icono,
                    Mensaje = estado.Mensaje,
                    Actual = estado.Id == estadoActualId,
                    Completado = estado.Id < estadoActualId || (estado.Id == estadoActualId && estado.Id == 4), // Completado si es menor al actual o si es "Completado"
                    FechaCompletado = estado.Id < estadoActualId ? servicio?.FechaActualizacion : null
                };

                flujoEstados.Add(estadoProgreso);
            }

            return flujoEstados;
        }

        private static int CalcularPorcentajeCompletado(int estadoActualId)
        {
            return estadoActualId switch
            {
                1 => 25,   // Pendiente
                2 => 50,   // En proceso
                3 => 75,   // En revisión
                4 => 100,  // Completado
                5 => 0,    // Cancelado
                _ => 0
            };
        }

        private static string GenerarMensajeEstado(int estadoActualId, string? mecanicoNombre)
        {
            return estadoActualId switch
            {
                1 => "Tu servicio ha sido creado y está en la cola de espera. Pronto será asignado a un mecánico.",
                2 => mecanicoNombre != null ? $"El mecánico {mecanicoNombre} está trabajando en tu vehículo." : "Un mecánico está trabajando en tu vehículo.",
                3 => "El servicio está siendo revisado para asegurar la calidad del trabajo realizado.",
                4 => "¡Tu servicio ha sido completado! Puedes pasar a recoger tu vehículo.",
                5 => "El servicio ha sido cancelado.",
                _ => "Estado desconocido"
            };
        }

        private async Task ValidarCapacidadTallerAsync()
        {
            const int CAPACIDAD_MAXIMA_TALLER = 15;
            
            // Obtener servicios pendientes (sin mecánico asignado)
            var serviciosPendientes = await _servicioRepository.GetActivosPendientesAsync(new[] { 1 }); // Estado "Pendiente"
            
            if (serviciosPendientes.Count() >= CAPACIDAD_MAXIMA_TALLER)
            {
                throw new InvalidOperationException($"La capacidad del taller está al límite. Actualmente hay {serviciosPendientes.Count()} servicios pendientes. La capacidad máxima es de {CAPACIDAD_MAXIMA_TALLER} servicios. Por favor, intente más tarde cuando se liberen espacios.");
            }
        }
    }
}

