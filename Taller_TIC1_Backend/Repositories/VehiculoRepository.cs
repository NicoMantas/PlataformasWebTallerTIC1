using Microsoft.EntityFrameworkCore;
using Taller_TIC1_Backend.Data;
using Taller_TIC1_Backend.Models;
using Taller_TIC1_Backend.Repositories.Interfaces;

namespace Taller_TIC1_Backend.Repositories
{
    public class VehiculoRepository : IVehiculoRepository
    {
        private readonly ApplicationDbContext _context;

        public VehiculoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Vehiculo>> GetAllAsync()
        {
            return await _context.Vehiculos.ToListAsync();
        }

        // Get a vehicle by its ID
        public async Task<Vehiculo?> GetByIdAsync(int id)
        {
            return await _context.Vehiculos
                .Include(v => v.VGasolina)
                .Include(v => v.VElectrico)
                .Include(v => v.VHibrido)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        // Create a new vehicle
        public async Task<Vehiculo> CreateAsync(Vehiculo vehiculo)
        {
            _context.Vehiculos.Add(vehiculo);
            await _context.SaveChangesAsync();
            return vehiculo;
        }

        public async Task<Vehiculo?> UpdateAsync(int id, Vehiculo vehiculo)
        {
            var existingVehiculo = await _context.Vehiculos.FindAsync(id);
            if (existingVehiculo == null)
                return null;

            existingVehiculo.Placa = vehiculo.Placa;
            existingVehiculo.Marca = vehiculo.Marca;
            existingVehiculo.Modelo = vehiculo.Modelo;
            existingVehiculo.Anio = vehiculo.Anio;

            await _context.SaveChangesAsync();
            return existingVehiculo;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo == null)
                return false;

            _context.Vehiculos.Remove(vehiculo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Vehiculos.AnyAsync(v => v.Id == id);
        }

        public async Task<Vehiculo?> GetByPlacaAsync(string placa)
        {
            return await _context.Vehiculos
                .FirstOrDefaultAsync(v => v.Placa == placa);
        }
    }
}
