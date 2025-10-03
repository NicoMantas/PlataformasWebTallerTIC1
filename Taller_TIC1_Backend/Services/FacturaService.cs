using AutoMapper;
using B_TallerAutomoviles.Clases;
using Taller_TIC1_Backend.Models;
using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Repositories;
using Taller_TIC1_Backend.Repositories.Interfaces;
using Taller_TIC1_Backend.Services.Interfaces;

namespace Taller_TIC1_Backend.Services
{
    public class FacturaService : IFacturaService
    {
        private readonly IFacturaRepository _facturaRepository;
        private readonly IOrdenTrabajoRepository _ordenTrabajoRepository;
        //private readonly IOrdenTrabajoRepository _ordenRepository; // Inyectar el repositorio de Orden para traer servicios de la orden
        private readonly IServicioRepository _servicioRepository; // Inyectar el repositorio de Servicio Para inspeccionar detalles (revisi�n/reparaci�n)
        private readonly IMapper _mapper;

        public FacturaService(IFacturaRepository facturaRepository, IOrdenTrabajoRepository ordenTrabajoRepository, IServicioRepository servicioRepository, IMapper mapper)
        {
            _facturaRepository = facturaRepository;
            _ordenTrabajoRepository = ordenTrabajoRepository;
            _servicioRepository = servicioRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FacturaDTO>> GetAllFacturasAsync()
        {
            var facturas = await _facturaRepository.GetAllAsync();
            var list = _mapper.Map<IEnumerable<FacturaDTO>>(facturas).ToList();
            foreach (var dto in list) { 

                await EnrichFacturaDtoAsync(dto);

            } return list;

        }

        public async Task<FacturaDTO?> GetFacturaByIdAsync(int id)
        {
            var factura = await _facturaRepository.GetByIdAsync(id);
            if (factura == null) return null;

            var dto = _mapper.Map<FacturaDTO>(factura);
            await EnrichFacturaDtoAsync(dto);
            return dto;
        }

        public async Task<FacturaDTO> CreateFacturaAsync(FacturaCreateDTO facturaDto)
        {
            // Reemplaza la línea problemática en CreateFacturaAsync:
            var factura = _mapper.Map<Taller_TIC1_Backend.Models.Factura>(facturaDto); //esta linea por corregir 
            var facturaCreada = await _facturaRepository.CreateAsync(factura);

            var dto = _mapper.Map<FacturaDTO>(facturaCreada);
            await EnrichFacturaDtoAsync(dto);
            return dto;
        }

        public async Task<FacturaDTO> UpdateFacturaAsync(int id, FacturaDTO facturaDto)
        {
            var facturaExistente = await _facturaRepository.GetByIdAsync(id);
            try
            {
                if (facturaExistente == null)
                    throw new ArgumentException("Factura no encontrada");

                _mapper.Map(facturaDto, facturaExistente);
                var facturaActualizada = await _facturaRepository.UpdateAsync(facturaExistente);
                return _mapper.Map<FacturaDTO>(facturaActualizada);

                var dto = _mapper.Map<FacturaDTO>(facturaActualizada);
                await EnrichFacturaDtoAsync(dto); 
                return dto;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la factura", ex);
            }
        }

        public async Task<bool> DeleteFacturaAsync(int id)
        {
            return await _facturaRepository.DeleteAsync(id);
        }

        public async Task<FacturaDTO?> GetFacturaByOrdenIdAsync(int ordenId)
        {
            var factura = await _facturaRepository.GetByOrdenIdAsync(ordenId);
            if (factura == null) return null;
            var dto = _mapper.Map<FacturaDTO>(factura);
            await EnrichFacturaDtoAsync(dto);
            return dto;
        }
        //metodo para enriquecer la fatura: Trae servicios, infiere "servicio realizado" y rellena cliente/placa
        private async Task EnrichFacturaDtoAsync(FacturaDTO dto) { 

                var servicio = (await _ordenTrabajoRepository.GetServiciosByOrdenIdAsync(dto.IdOrdenTrabajo)).ToList();
    var primero = servicio.FirstOrDefault();

    if (primero != null) {
        dto.ClienteNombre = primero.Cliente?.Nombre ?? "Sin cliente";
        dto.VehiculoPlaca = primero.Cliente?.Vehiculo?.Placa ?? "Sin placa";
    }
            var descripciones = new List<string>();
            foreach (var s in servicio)
            {
                // �Es revisi�n?
                var rev = await _servicioRepository.GetDetalleRevisionByServicioIdAsync(s.Id);
                if (rev != null)
                {
                    // Incluir detalles textuales de la revisi�n
                    descripciones.Add($"Revision: {rev.Detalles}");
                    continue;
                }

                // �Es reparaci�n?
                var rep = await _servicioRepository.GetDetalleReparacionByServicioIdAsync(s.Id);
                if (rep != null)
                {
                    var repuestos = await _servicioRepository.GetRepuestosByDetalleReparacionAsync(rep.IdDetalleReparacionRepuesto);
                    var count = repuestos.Count();
                    descripciones.Add($"Reparacion: {count} repuesto(s)");
                    continue;
                }

                // Si no hay detalle, dejar gen�rico
                descripciones.Add("Servicio");
            }

            dto.ServiciosRealizados = descripciones;
        }




    }
}

