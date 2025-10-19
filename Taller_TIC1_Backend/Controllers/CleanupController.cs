using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Taller_TIC1_Backend.Data;

namespace Taller_TIC1_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CleanupController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CleanupController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpDelete("all")]
        public async Task<ActionResult> CleanupAllTables()
        {
            try
            {
                // Deshabilitar las restricciones de clave foránea temporalmente
                await _context.Database.ExecuteSqlRawAsync("SET session_replication_role = replica;");

                // Lista de tablas a limpiar (nombres correctos de la BD)
                var tablesToClean = new[]
                {
                    "DetalleServicioOrden",
                    "OrdenTrabajo", 
                    "DetalleReparacionRepuesto",
                    "DetalleReparacion",
                    "DetalleRevision",
                    "Servicio",
                    "Factura",
                    "UsuariosClienteTaller",
                    "UsuariosEmpleadoTaller",
                    "CEmpresa",
                    "CNatural",
                    "Cliente",
                    "VHibrido",
                    "VElectrico",
                    "VGasolina",
                    "Vehiculo",
                    "Empleado",
                    "RepuestoProveedor",
                    "Repuesto",
                    "Proveedor",
                    "EstadoServicio",
                    "TipoEmpleado",
                    "TipoEstadoOrden",
                    "Taller"
                };

                var cleanedTables = new List<string>();
                var errors = new List<string>();

                foreach (var table in tablesToClean)
                {
                    try
                    {
                        await _context.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE \"{table}\" CASCADE;");
                        cleanedTables.Add(table);
                    }
                    catch (Exception ex)
                    {
                        // Si la tabla no existe, continuar con la siguiente
                        if (ex.Message.Contains("does not exist"))
                        {
                            continue; // Saltar esta tabla
                        }
                        errors.Add($"{table}: {ex.Message}");
                    }
                }

                // Restaurar las restricciones de clave foránea
                await _context.Database.ExecuteSqlRawAsync("SET session_replication_role = DEFAULT;");

                var result = new
                {
                    message = "Limpieza completada",
                    cleanedTables = cleanedTables,
                    errors = errors,
                    totalCleaned = cleanedTables.Count
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al limpiar las tablas: {ex.Message}");
            }
        }

        [HttpDelete("test-data")]
        public async Task<ActionResult> CleanupTestData()
        {
            try
            {
                // Limpiar solo datos de prueba (mantener datos de configuración)
                await _context.Database.ExecuteSqlRawAsync("SET session_replication_role = replica;");

                // Tablas de datos de prueba (no configuración)
                var testDataTables = new[]
                {
                    "DetalleServicioOrden",
                    "OrdenTrabajo",
                    "DetalleReparacionRepuesto", 
                    "DetalleReparacion",
                    "DetalleRevision",
                    "Servicio",
                    "Factura",
                    "UsuariosClienteTaller",
                    "UsuariosEmpleadoTaller",
                    "CEmpresa",
                    "CNatural", 
                    "Cliente",
                    "VHibrido",
                    "VElectrico",
                    "VGasolina",
                    "Vehiculo",
                    "Empleado",
                    "RepuestoProveedor",
                    "Repuesto",
                    "Proveedor"
                };

                var cleanedTables = new List<string>();
                var errors = new List<string>();

                foreach (var table in testDataTables)
                {
                    try
                    {
                        await _context.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE \"{table}\" CASCADE;");
                        cleanedTables.Add(table);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("does not exist"))
                        {
                            continue; // Saltar esta tabla
                        }
                        errors.Add($"{table}: {ex.Message}");
                    }
                }

                await _context.Database.ExecuteSqlRawAsync("SET session_replication_role = DEFAULT;");

                var result = new
                {
                    message = "Datos de prueba limpiados exitosamente. Se mantuvieron las tablas de configuración.",
                    cleanedTables = cleanedTables,
                    errors = errors,
                    totalCleaned = cleanedTables.Count
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al limpiar los datos de prueba: {ex.Message}");
            }
        }

        [HttpGet("status")]
        public async Task<ActionResult> GetDatabaseStatus()
        {
            try
            {
                var status = new
                {
                    Clientes = await _context.Clientes.CountAsync(),
                    Vehiculos = await _context.Vehiculos.CountAsync(),
                    Empleados = await _context.Empleados.CountAsync(),
                    Servicios = await _context.Servicios.CountAsync(),
                    Repuestos = await _context.Repuestos.CountAsync(),
                    Proveedores = await _context.Proveedores.CountAsync(),
                    UsuariosCliente = await _context.UsuariosClienteTaller.CountAsync(),
                    UsuariosEmpleado = await _context.UsuariosEmpleadoTaller.CountAsync(),
                    Talleres = await _context.Talleres.CountAsync()
                };

                return Ok(status);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener el estado de la base de datos: {ex.Message}");
            }
        }
    }
}
