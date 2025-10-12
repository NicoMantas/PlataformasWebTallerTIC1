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
        private readonly IServicioRepository _servicioRepository;
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
            var factura = _mapper.Map<Taller_TIC1_Backend.Models.Factura>(facturaDto);
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

        public async Task<FacturaDTO?> GetFacturaByServicioIdAsync(int servicioId)
        {
            // First get the order associated with this service
            var orden = await _servicioRepository.GetOrdenByServicioIdAsync(servicioId);
            if (orden == null) return null;
            
            // Then get the invoice for that order
            var factura = await _facturaRepository.GetByOrdenIdAsync(orden.Id);
            if (factura == null) return null;
            
            var dto = _mapper.Map<FacturaDTO>(factura);
            await EnrichFacturaDtoAsync(dto);
            return dto;
        }

        //metodo para enriquecer la factura: Trae servicios, infiere "servicio realizado", rellena cliente/placa y calcula costo real
        private async Task EnrichFacturaDtoAsync(FacturaDTO dto) { 

            var servicios = (await _ordenTrabajoRepository.GetServiciosByOrdenIdAsync(dto.IdOrdenTrabajo)).ToList();
            var primero = servicios.FirstOrDefault();

            if (primero != null) {
                dto.ClienteNombre = primero.Cliente?.Nombre ?? "Sin cliente";
                dto.VehiculoPlaca = primero.Cliente?.Vehiculo?.Placa ?? "Sin placa";
            }

            var descripciones = new List<string>();
            decimal subtotalReal = 0;

            foreach (var servicio in servicios)
            {
                // ¿Es revisión? - Costo fijo
                var rev = await _servicioRepository.GetDetalleRevisionByServicioIdAsync(servicio.Id);
                if (rev != null)
                {
                    var descripcionRevision = $"Revisión: {rev.Detalles}";
                    if (!string.IsNullOrEmpty(rev.DetallesEncontrados))
                    {
                        descripcionRevision += $" | Hallazgos: {rev.DetallesEncontrados}";
                    }
                    descripciones.Add(descripcionRevision);
                    subtotalReal += servicio.Costo; // Costo fijo para revisiones
                    continue;
                }

                // ¿Es reparación? - Costo base + repuestos
                var rep = await _servicioRepository.GetDetalleReparacionByServicioIdAsync(servicio.Id);
                if (rep != null)
                {
                    var repuestos = await _servicioRepository.GetRepuestosByDetalleReparacionAsync(rep.IdDetalleReparacionRepuesto);
                    var costoRepuestos = repuestos.Sum(r => r.Cantidad * r.Repuesto.Precio);
                    var costoTotal = servicio.Costo + costoRepuestos; // Costo base + repuestos
                    
                    descripciones.Add($"Reparación: {repuestos.Count()} repuesto(s) - Total: ${costoTotal:N0}");
                    subtotalReal += costoTotal;
                    continue;
                }

                // Si no hay detalle, usar costo base
                descripciones.Add("Servicio");
                subtotalReal += servicio.Costo;
            }

            dto.ServiciosRealizados = descripciones;
            
            // Actualizar los valores de la factura con el costo real
            dto.Subtotal = subtotalReal;
            dto.Impuestos = subtotalReal * 0.19m; // 19% IVA
            dto.Total = subtotalReal + dto.Impuestos;
        }
    }
}
