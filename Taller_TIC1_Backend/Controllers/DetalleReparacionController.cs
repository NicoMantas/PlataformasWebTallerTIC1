using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Taller_TIC1_Backend.Data;
using Taller_TIC1_Backend.Models;

namespace Taller_TIC1_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetalleReparacionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DetalleReparacionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/DetalleReparacion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetalleReparacion>>> GetReparaciones()
        {
            var items = await _context.DetallesReparacion
                .Include(dr => dr.Servicio)
                .Include(dr => dr.DetalleReparacionRepuesto)
                .ToListAsync();
            return Ok(items);
        }

        // GET: api/DetalleReparacion/5  (idServicio)
        [HttpGet("{id:int}")]
        public async Task<ActionResult<DetalleReparacion>> GetReparacion(int id)
        {
            var entity = await _context.DetallesReparacion
                .Include(dr => dr.Servicio)
                .Include(dr => dr.DetalleReparacionRepuesto)
                .FirstOrDefaultAsync(x => x.IdServicio == id);
            if (entity == null) return NotFound();
            return Ok(entity);
        }

        // POST: api/DetalleReparacion
        [HttpPost]
        public async Task<ActionResult<DetalleReparacion>> PostReparacion(DetalleReparacion dto)
        {
            // Validar que el servicio exista
            var servicioExists = await _context.Servicios.AnyAsync(s => s.Id == dto.IdServicio);
            if (!servicioExists) return BadRequest("El servicio especificado no existe.");

            // Validar que no exista ya una reparación para ese servicio (1:1)
            var exists = await _context.DetallesReparacion.AnyAsync(r => r.IdServicio == dto.IdServicio);
            if (exists) return BadRequest("Ya existe una reparación para este servicio.");

            _context.DetallesReparacion.Add(dto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReparacion), new { id = dto.IdServicio }, dto);
        }

        // PUT: api/DetalleReparacion/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutReparacion(int id, DetalleReparacion dto)
        {
            if (id != dto.IdServicio) return BadRequest();

            var entity = await _context.DetallesReparacion.FindAsync(id);
            if (entity == null) return NotFound();

            entity.IdDetalleReparacionRepuesto = dto.IdDetalleReparacionRepuesto;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/DetalleReparacion/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteReparacion(int id)
        {
            var entity = await _context.DetallesReparacion.FindAsync(id);
            if (entity == null) return NotFound();

            _context.DetallesReparacion.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/DetalleReparacion/{id}/repuestos
        [HttpGet("{id:int}/repuestos")]
        public async Task<ActionResult<IEnumerable<DetalleReparacionRepuesto>>> GetRepuestosByReparacion(int id)
        {
            // Buscar el detalle de reparación para este servicio
            var detalleReparacion = await _context.DetallesReparacion
                .FirstOrDefaultAsync(dr => dr.IdServicio == id);
            
            if (detalleReparacion == null)
            {
                // Si no existe detalle de reparación, devolver lista vacía
                return Ok(new List<DetalleReparacionRepuesto>());
            }

            // Buscar el repuesto relacionado con este detalle de reparación
            var repuesto = await _context.DetallesReparacionRepuesto
                .Include(drr => drr.Repuesto)
                .FirstOrDefaultAsync(drr => drr.Id == detalleReparacion.IdDetalleReparacionRepuesto);

            var repuestos = new List<DetalleReparacionRepuesto>();
            if (repuesto != null)
            {
                repuestos.Add(repuesto);
            }

            return Ok(repuestos);
        }

        // POST: api/DetalleReparacion/{id}/repuestos
        [HttpPost("{id:int}/repuestos")]
        public async Task<ActionResult<object>> AddRepuestoToReparacion(int id, [FromBody] AddRepuestoDto dto)
        {
            try
            {
                // Verificar que el servicio existe
                var servicio = await _context.Servicios.FindAsync(id);
                if (servicio == null) return NotFound("Servicio no encontrado");

                // Verificar que el repuesto existe y tiene stock suficiente
                var repuesto = await _context.Repuestos.FindAsync(dto.IdRepuesto);
                if (repuesto == null) return NotFound("Repuesto no encontrado");
                if (repuesto.Stock < dto.Cantidad) return BadRequest($"Stock insuficiente. Disponible: {repuesto.Stock}, Solicitado: {dto.Cantidad}");

                // Verificar si ya existe un detalle de reparación para este servicio
                var detalleReparacionExistente = await _context.DetallesReparacion
                .FirstOrDefaultAsync(dr => dr.IdServicio == id);
            
                DetalleReparacionRepuesto nuevoRepuesto;
                decimal costoAnterior = 0;

                if (detalleReparacionExistente == null)
                {
                    // Obtener el siguiente ID disponible
                    var maxId = await _context.DetallesReparacionRepuesto
                        .MaxAsync(drr => (int?)drr.Id) ?? 0;
                    var nuevoId = maxId + 1;

                    // Crear nuevo repuesto
                    nuevoRepuesto = new DetalleReparacionRepuesto
                    {
                        Id = nuevoId,
                        IdRepuesto = dto.IdRepuesto,
                        Cantidad = dto.Cantidad
                    };

                    _context.DetallesReparacionRepuesto.Add(nuevoRepuesto);
                    await _context.SaveChangesAsync();

                    // Crear detalle de reparación
                    var nuevoDetalleReparacion = new DetalleReparacion
                    {
                        IdServicio = id,
                        IdDetalleReparacionRepuesto = nuevoRepuesto.Id
                    };

                    _context.DetallesReparacion.Add(nuevoDetalleReparacion);
                }
                else
                {
                    // Buscar el repuesto existente
                    var repuestoExistente = await _context.DetallesReparacionRepuesto
                        .FirstOrDefaultAsync(drr => drr.Id == detalleReparacionExistente.IdDetalleReparacionRepuesto);

                    if (repuestoExistente != null)
                    {
                        // Calcular costo anterior
                        var repuestoAnterior = await _context.Repuestos.FindAsync(repuestoExistente.IdRepuesto);
                        if (repuestoAnterior != null)
                        {
                            costoAnterior = repuestoAnterior.Precio * repuestoExistente.Cantidad;
                        }

                        // Si es el mismo repuesto, sumar cantidad
                        if (repuestoExistente.IdRepuesto == dto.IdRepuesto)
                        {
                            repuestoExistente.Cantidad += dto.Cantidad;
                            nuevoRepuesto = repuestoExistente;
                        }
                        else
                        {
                            return BadRequest("Esta reparación ya tiene un repuesto asignado. Solo se permite un repuesto por reparación.");
                        }
                    }
                    else
                    {
                        // Obtener el siguiente ID disponible
                        var maxId = await _context.DetallesReparacionRepuesto
                            .MaxAsync(drr => (int?)drr.Id) ?? 0;
                        var nuevoId = maxId + 1;

                        // Crear nuevo repuesto para reparación existente
                        nuevoRepuesto = new DetalleReparacionRepuesto
                        {
                            Id = nuevoId,
                IdRepuesto = dto.IdRepuesto,
                Cantidad = dto.Cantidad
            };

                        _context.DetallesReparacionRepuesto.Add(nuevoRepuesto);
                        await _context.SaveChangesAsync();

                        detalleReparacionExistente.IdDetalleReparacionRepuesto = nuevoRepuesto.Id;
                    }
                }

                // Calcular nuevo costo
                decimal nuevoCosto = repuesto.Precio * dto.Cantidad;

                // Actualizar costo del servicio
                servicio.Costo = servicio.Costo - costoAnterior + nuevoCosto;

                // Reducir stock
                repuesto.Stock -= dto.Cantidad;

                // Guardar cambios
            await _context.SaveChangesAsync();

                // Incluir repuesto en respuesta
                nuevoRepuesto.Repuesto = repuesto;

                var response = new
                {
                    detalleReparacionRepuesto = nuevoRepuesto,
                    costoTotalServicio = servicio.Costo,
                    stockActualizado = repuesto.Stock
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log más detallado del error
                var innerException = ex.InnerException?.Message ?? "Sin inner exception";
                var stackTrace = ex.StackTrace ?? "Sin stack trace";
                
                return StatusCode(500, $"Error interno del servidor: {ex.Message}\nInner Exception: {innerException}\nStackTrace: {stackTrace}");
            }
        }

        // PUT: api/DetalleReparacion/{id}/repuestos
        [HttpPut("{id:int}/repuestos")]
        public async Task<ActionResult<object>> UpdateRepuestoInReparacion(int id, [FromBody] AddRepuestoDto dto)
        {
            try
            {
                // Verificar que el servicio existe
                var servicio = await _context.Servicios.FindAsync(id);
                if (servicio == null) return NotFound("Servicio no encontrado");

                // Verificar que el repuesto existe y tiene stock suficiente
                var repuesto = await _context.Repuestos.FindAsync(dto.IdRepuesto);
                if (repuesto == null) return NotFound("Repuesto no encontrado");

                // Buscar el detalle de reparación
                var detalleReparacion = await _context.DetallesReparacion
                    .FirstOrDefaultAsync(dr => dr.IdServicio == id);
                
                if (detalleReparacion == null) return NotFound("No se encontró detalle de reparación");

                // Buscar el repuesto existente
                var repuestoExistente = await _context.DetallesReparacionRepuesto
                    .FirstOrDefaultAsync(drr => drr.Id == detalleReparacion.IdDetalleReparacionRepuesto);

                if (repuestoExistente == null) return NotFound("No se encontró repuesto en esta reparación");

                // Obtener el repuesto anterior para calcular costos
                var repuestoAnterior = await _context.Repuestos.FindAsync(repuestoExistente.IdRepuesto);
                decimal costoAnterior = 0;
                if (repuestoAnterior != null)
                {
                    costoAnterior = repuestoAnterior.Precio * repuestoExistente.Cantidad;
                }

                // Calcular la diferencia de stock
                var diferenciaStock = dto.Cantidad - repuestoExistente.Cantidad;
                
                // Verificar que hay suficiente stock disponible
                if (repuesto.Stock < diferenciaStock) 
                    return BadRequest($"Stock insuficiente. Disponible: {repuesto.Stock}, Necesario: {diferenciaStock}");

                // Calcular el nuevo costo
                decimal nuevoCosto = repuesto.Precio * dto.Cantidad;

                // Actualizar el costo del servicio
                servicio.Costo = servicio.Costo - costoAnterior + nuevoCosto;

                // Actualizar la cantidad y repuesto
                repuestoExistente.Cantidad = dto.Cantidad;
                repuestoExistente.IdRepuesto = dto.IdRepuesto;

                // Actualizar el stock del repuesto anterior (devolver stock)
                if (repuestoAnterior != null && repuestoAnterior.Id != repuesto.Id)
                {
                    repuestoAnterior.Stock += repuestoExistente.Cantidad;
                }

                // Actualizar el stock del nuevo repuesto
                repuesto.Stock -= diferenciaStock;

                // Guardar todos los cambios de una vez
                await _context.SaveChangesAsync();

                // Incluir el repuesto en la respuesta
                repuestoExistente.Repuesto = repuesto;

                // Crear respuesta con información adicional
                var response = new
                {
                    detalleReparacionRepuesto = repuestoExistente,
                    costoTotalServicio = servicio.Costo,
                    stockActualizado = repuesto.Stock
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}

public class AddRepuestoDto
{
    public int IdRepuesto { get; set; }
    public int Cantidad { get; set; }
}