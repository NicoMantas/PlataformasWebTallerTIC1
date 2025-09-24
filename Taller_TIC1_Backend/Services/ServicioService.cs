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
                // Aquí necesitarías un repositorio para DetalleRevision
            }
            else if (servicioDto.TipoServicio == "Reparacion" && servicioDto.RepuestosReparacion != null)
            {
                // Lógica para crear detalles de reparación
                // Esto sería más complejo y requeriría transacciones
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
    }
}

