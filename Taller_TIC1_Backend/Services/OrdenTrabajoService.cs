using AutoMapper;
using B_TallerAutomoviles.Clases;
using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Repositories.Interfaces;
using Taller_TIC1_Backend.Services.Interfaces;

namespace Taller_TIC1_Backend.Services
{
    public class OrdenTrabajoService : IOrdenTrabajoService
    {
        private readonly IOrdenTrabajoRepository _ordenRepository;
        private readonly IMapper _mapper;

        public OrdenTrabajoService(IOrdenTrabajoRepository ordenRepository, IMapper mapper)
        {
            _ordenRepository = ordenRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrdenTrabajoDTO>> GetAllOrdenesAsync()
        {
            var ordenes = await _ordenRepository.GetAllAsync();
            var ordenesDto = new List<OrdenTrabajoDTO>();

            foreach (var orden in ordenes)
            {
                var ordenDto = _mapper.Map<OrdenTrabajoDTO>(orden);
                var servicios = await _ordenRepository.GetServiciosByOrdenIdAsync(orden.Id);
                ordenDto.ServiciosIds = servicios.Select(s => s.Id).ToList();
                
                // Cargar información adicional
                ordenDto.EstadoDescripcion = orden.TipoEstadoOrden?.Descripcion ?? "Sin estado";
                ordenDto.ClienteNombre = "Cliente"; // Se puede mejorar con join
                ordenDto.VehiculoPlaca = "Placa"; // Se puede mejorar con join
                
                ordenesDto.Add(ordenDto);
            }

            return ordenesDto;
        }

        public async Task<OrdenTrabajoDTO?> GetOrdenByIdAsync(int id)
        {
            var orden = await _ordenRepository.GetByIdAsync(id);
            if (orden == null) return null;

            var ordenDto = _mapper.Map<OrdenTrabajoDTO>(orden);
            var servicios = await _ordenRepository.GetServiciosByOrdenIdAsync(id);
            ordenDto.ServiciosIds = servicios.Select(s => s.Id).ToList();

            return ordenDto;
        }

        public async Task<OrdenTrabajoDTO> CreateOrdenAsync(OrdenTrabajoCreateDTO ordenDto)
        {
            var orden = _mapper.Map<Models.OrdenDeTrabajo>(ordenDto);
            var ordenCreada = await _ordenRepository.CreateAsync(orden);

            // Agregar servicios a la orden
            foreach (var servicioId in ordenDto.ServiciosIds)
            {
                await _ordenRepository.AddServicioToOrdenAsync(ordenCreada.Id, servicioId);
            }

            return await GetOrdenByIdAsync(ordenCreada.Id);
        }

        public async Task<OrdenTrabajoDTO> UpdateOrdenAsync(int id, OrdenTrabajoDTO ordenDto)
        {
            var ordenExistente = await _ordenRepository.GetByIdAsync(id);
            if (ordenExistente == null)
                throw new ArgumentException("Orden no encontrada");

            _mapper.Map(ordenDto, ordenExistente);
            var ordenActualizada = await _ordenRepository.UpdateAsync(ordenExistente);

            // Actualizar servicios (lógica simplificada)
            var serviciosActuales = await _ordenRepository.GetServiciosByOrdenIdAsync(id);
            var serviciosActualesIds = serviciosActuales.Select(s => s.Id).ToList();

            // Agregar nuevos servicios
            foreach (var servicioId in ordenDto.ServiciosIds.Except(serviciosActualesIds))
            {
                await _ordenRepository.AddServicioToOrdenAsync(id, servicioId);
            }

            // Remover servicios eliminados
            foreach (var servicioId in serviciosActualesIds.Except(ordenDto.ServiciosIds))
            {
                await _ordenRepository.RemoveServicioFromOrdenAsync(id, servicioId);
            }

            return await GetOrdenByIdAsync(id);
        }

        public async Task<bool> DeleteOrdenAsync(int id)
        {
            return await _ordenRepository.DeleteAsync(id);
        }

        public async Task<bool> AddServicioToOrdenAsync(int ordenId, int servicioId)
        {
            return await _ordenRepository.AddServicioToOrdenAsync(ordenId, servicioId);
        }

        public async Task<bool> RemoveServicioFromOrdenAsync(int ordenId, int servicioId)
        {
            return await _ordenRepository.RemoveServicioFromOrdenAsync(ordenId, servicioId);
        }
    }
}

