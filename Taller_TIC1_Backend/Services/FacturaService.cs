using AutoMapper;
using Taller_TIC1_Backend.Models;
using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Repositories.Interfaces;
using Taller_TIC1_Backend.Services.Interfaces;

namespace Taller_TIC1_Backend.Services
{
    public class FacturaService : IFacturaService
    {
        private readonly IFacturaRepository _facturaRepository;
        private readonly IMapper _mapper;

        public FacturaService(IFacturaRepository facturaRepository, IMapper mapper)
        {
            _facturaRepository = facturaRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FacturaDTO>> GetAllFacturasAsync()
        {
            var facturas = await _facturaRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<FacturaDTO>>(facturas);
        }

        public async Task<FacturaDTO?> GetFacturaByIdAsync(int id)
        {
            var factura = await _facturaRepository.GetByIdAsync(id);
            if (factura == null) return null;
            return _mapper.Map<FacturaDTO>(factura);
        }

        public async Task<FacturaDTO> CreateFacturaAsync(FacturaCreateDTO facturaDto)
        {
            var factura = _mapper.Map<Factura>(facturaDto);
            var facturaCreada = await _facturaRepository.CreateAsync(factura);
            return _mapper.Map<FacturaDTO>(facturaCreada);
        }

        public async Task<FacturaDTO> UpdateFacturaAsync(int id, FacturaDTO facturaDto)
        {
            var facturaExistente = await _facturaRepository.GetByIdAsync(id);
            if (facturaExistente == null)
                throw new ArgumentException("Factura no encontrada");

            _mapper.Map(facturaDto, facturaExistente);
            var facturaActualizada = await _facturaRepository.UpdateAsync(facturaExistente);
            return _mapper.Map<FacturaDTO>(facturaActualizada);
        }

        public async Task<bool> DeleteFacturaAsync(int id)
        {
            return await _facturaRepository.DeleteAsync(id);
        }

        public async Task<FacturaDTO?> GetFacturaByOrdenIdAsync(int ordenId)
        {
            var factura = await _facturaRepository.GetByOrdenIdAsync(ordenId);
            if (factura == null) return null;
            return _mapper.Map<FacturaDTO>(factura);
        }
    }
}
