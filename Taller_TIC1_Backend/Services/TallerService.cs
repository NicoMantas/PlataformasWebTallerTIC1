using Microsoft.EntityFrameworkCore;
using Taller_TIC1_Backend.Data;
using Taller_TIC1_Backend.Models;
using Taller_TIC1_Backend.Models.DTOs;
using Taller_TIC1_Backend.Repositories.Interfaces;
using Taller_TIC1_Backend.Services.Interfaces;
namespace Taller_TIC1_Backend.Services
{
    public class TallerService: ITallerService
    {
        private readonly ITallerRepository _tallerRepository;
        private readonly ApplicationDbContext _context;

        public TallerService(ITallerRepository tallerRepository, ApplicationDbContext context)
        {
            _tallerRepository = tallerRepository;
            _context = context;
        }
        // Método para generar el próximo ID disponible para un nuevo taller
        private async Task<int> GetNextIdAsync()
        {
            var allTalleres = await _context.Talleres.ToListAsync();
            if (!allTalleres.Any())
                return 1;

            return allTalleres.Max(t => t.Id) + 1;
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
            // Obtener el próximo ID disponible
            var nextId = await GetNextIdAsync();
            var taller = new Taller
            {
                Id = nextId,
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

        public async Task<TallerResponseDto?> UpdateAsync(int id, TallerUpdateDto tallerUpdateDto)
        {
            var taller = await _tallerRepository.GetByIdAsync(id);
            if (taller == null)
                return null;

            taller.Nombre = tallerUpdateDto.Nombre;
            taller.Direccion = tallerUpdateDto.Direccion;

            // Actualizar en la base de datos
            _context.Talleres.Update(taller);
            await _context.SaveChangesAsync();

            return MapToResponseDto(taller);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var taller = await _tallerRepository.GetByIdAsync(id);
            if (taller == null)
                return false;

            _context.Talleres.Remove(taller);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
