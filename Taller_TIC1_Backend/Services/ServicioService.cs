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

        public async Task<ServicioDTO> CreateServicioAsync(ServicioCreateDTO servicioDto)
        {
            var servicio = _mapper.Map<Servicio>(servicioDto);
            var servicioCreado = await _servicioRepository.CreateAsync(servicio);

            // Crear detalles específicos según el tipo de servicio
            if (servicioDto.TipoServicio == "Revision" && !string.IsNullOrEmpty(servicioDto.DetallesRevision))
            {
                var detalleRevision = new DetalleRevision
                {
                    IdServicio = servicioCreado.Id,
                    Detalles = servicioDto.DetallesRevision
                };
                await _servicioRepository.CreateDetalleRevisionAsync(detalleRevision);
            }
            else if (servicioDto.TipoServicio == "Reparacion" && servicioDto.RepuestosReparacion != null && servicioDto.RepuestosReparacion.Count > 0)
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

            return _mapper.Map<ServicioDTO>(servicioCreado);
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
            // Activos: Pendiente, En proceso
            var pendienteId = await _servicioRepository.GetEstadoIdByDescripcionAsync("Pendiente");
            var enProcesoId = await _servicioRepository.GetEstadoIdByDescripcionAsync("En proceso");
            var ids = new List<int>();
            if (pendienteId.HasValue) ids.Add(pendienteId.Value);
            if (enProcesoId.HasValue) ids.Add(enProcesoId.Value);
            var servicios = await _servicioRepository.GetByClienteAndEstadosAsync(clienteId, ids);
            return await MapDetallesServicios(servicios);
        }

        public async Task<IEnumerable<ServicioDTO>> GetServiciosHistorialByClienteAsync(int clienteId)
        {
            // Historial: solo Completado
            var completadoId = await _servicioRepository.GetEstadoIdByDescripcionAsync("Completado");
            var ids = new List<int>();
            if (completadoId.HasValue) ids.Add(completadoId.Value);
            var servicios = await _servicioRepository.GetByClienteAndEstadosAsync(clienteId, ids);
            return await MapDetallesServicios(servicios);
        }

        public async Task<bool> CancelarServicioAsync(int id)
        {
            var servicio = await _servicioRepository.GetByIdAsync(id);
            if (servicio == null) return false;
            var canceladoId = await _servicioRepository.GetEstadoIdByDescripcionAsync("Cancelado");
            var ensureId = canceladoId ?? await _servicioRepository.EnsureEstadoAsync("Cancelado");
            servicio.IdEstado = ensureId;
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
                dto.VehiculoPlaca = servicio.Cliente?.Vehiculo?.Placa ?? "";
                dto.VehiculoMarca = servicio.Cliente?.Vehiculo?.Marca ?? "";
                dto.VehiculoModelo = servicio.Cliente?.Vehiculo?.Modelo ?? "";
                dto.FechaCreacion = servicio.FechaCreacion;
                dto.FechaActualizacion = servicio.FechaActualizacion;

                var detalleRevision = await _servicioRepository.GetDetalleRevisionByServicioIdAsync(servicio.Id);
                if (detalleRevision != null)
                {
                    dto.TipoServicio = "Revision";
                    dto.DetallesRevision = detalleRevision.Detalles;
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
            var pendienteId = await _servicioRepository.GetEstadoIdByDescripcionAsync("Pendiente");
            var enProcesoId = await _servicioRepository.GetEstadoIdByDescripcionAsync("En proceso");
            var ids = new List<int>();
            if (pendienteId.HasValue) ids.Add(pendienteId.Value);
            if (enProcesoId.HasValue) ids.Add(enProcesoId.Value);
            var servicios = await _servicioRepository.GetActivosPendientesAsync(ids);
            return await MapDetallesServicios(servicios);
        }

        public async Task<IEnumerable<ServicioDTO>> SecretariaListAsignadosAsync()
        {
            var pendienteId = await _servicioRepository.GetEstadoIdByDescripcionAsync("Pendiente");
            var enProcesoId = await _servicioRepository.GetEstadoIdByDescripcionAsync("En proceso");
            var ids = new List<int>();
            if (pendienteId.HasValue) ids.Add(pendienteId.Value);
            if (enProcesoId.HasValue) ids.Add(enProcesoId.Value);
            var servicios = await _servicioRepository.GetActivosAsignadosAsync(ids);
            return await MapDetallesServicios(servicios);
        }

        public async Task<bool> SecretariaAsignarMecanicoAsync(int servicioId, int empleadoId)
        {
            return await _servicioRepository.AssignEmpleadoAsync(servicioId, empleadoId);
        }

        public async Task<IEnumerable<ServicioDTO>> GetServiciosByMecanicoAsync(int empleadoId)
        {
            // Servicios asignados y en proceso (no completados)
            var pendienteId = await _servicioRepository.GetEstadoIdByDescripcionAsync("Pendiente");
            var enProcesoId = await _servicioRepository.GetEstadoIdByDescripcionAsync("En proceso");
            var ids = new List<int>();
            if (pendienteId.HasValue) ids.Add(pendienteId.Value);
            if (enProcesoId.HasValue) ids.Add(enProcesoId.Value);
            var servicios = await _servicioRepository.GetServiciosByEmpleadoAndEstadosAsync(empleadoId, ids);
            return await MapDetallesServicios(servicios);
        }

        public async Task<IEnumerable<ServicioDTO>> GetServiciosCompletadosByMecanicoAsync(int empleadoId)
        {
            // Servicios completados por el mecánico
            var completadoId = await _servicioRepository.GetEstadoIdByDescripcionAsync("Completado");
            var ids = new List<int>();
            if (completadoId.HasValue) ids.Add(completadoId.Value);
            var servicios = await _servicioRepository.GetServiciosByEmpleadoAndEstadosAsync(empleadoId, ids);
            return await MapDetallesServicios(servicios);
        }

        public async Task<bool> UpdateServicioEstadoAsync(int servicioId, int estadoId)
        {
            return await _servicioRepository.UpdateServicioEstadoAsync(servicioId, estadoId);
        }
    }
}

