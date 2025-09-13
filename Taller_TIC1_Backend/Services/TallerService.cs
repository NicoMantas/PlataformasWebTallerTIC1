using Taller_TIC1_Backend.Models;
using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Repositories.Interfaces;
using Taller_TIC1_Backend.Services.Interfaces;


namespace Taller_TIC1_Backend.Services
{
    public class TallerService: ITallerService
    {
        private readonly ITallerRepository _tallerRepository;

        public TallerService(ITallerRepository tallerRepository)
        {
            _tallerRepository = tallerRepository;
        }

        public async Task<IEnumerable<TallerResponseDto>> GetAllAsync()
        {
            var talleres = await _tallerRepository.GetAllAsync();
            return talleres.Select(MapToResponseDto);
        }

        public async Task<TallerResponseDto?> GetByIdAsync(int id)
        {
            var taller = await _tallerRepository.GetByIdAsync(id);
            return taller != null ? MapToResponseDto(taller) : null;
        }

        public async Task<TallerResponseDto> CreateAsync(TallerCreateDto tallerCreateDto)
        {
            var taller = new Taller
            {
                Nombre = tallerCreateDto.Nombre,
                Direccion = tallerCreateDto.Direccion
            };

            var createdTaller = await _tallerRepository.CreateAsync(taller);
            return MapToResponseDto(createdTaller);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _tallerRepository.ExistsAsync(id);
        }

        private static TallerResponseDto MapToResponseDto(Taller taller)
        {
            return new TallerResponseDto
            {
                Id = taller.Id,
                Nombre = taller.Nombre,
                Direccion = taller.Direccion
            };
        }
    }
}
