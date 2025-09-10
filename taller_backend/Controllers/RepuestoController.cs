using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using taller_backend.ContextDB;
using taller_backend.DTOs;
using taller_backend.Models;


namespace taller_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepuestoController : Controller
    {
        private readonly ApplicationDbContext _ctx;

        public RepuestoController(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }


        // GET api/Repuestos?includeProveedor=true
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Repuesto>>> GetAll([FromQuery] bool includeProveedor = false)
        {
            IQueryable<Repuesto> q = _ctx.Repuestos;
            if (includeProveedor) q = q.Include(r => r.Proveedor);
            var list = await q.ToListAsync();
            return Ok(list);
        }

        // GET api/Repuestos/5
        [HttpGet("{id:long}")]
        public async Task<ActionResult<Repuesto>> GetById(long id, [FromQuery] bool includeProveedor = false)
        {
            IQueryable<Repuesto> q = _ctx.Repuestos;
            if (includeProveedor) q = q.Include(r => r.Proveedor);

            var entity = await q.FirstOrDefaultAsync(r => r.Id == id);
            if (entity == null) return NotFound();
            return Ok(entity);
        }

        // GET api/Repuestos/by-proveedor/10
        [HttpGet("by-proveedor/{proveedorId:long}")]
        public async Task<ActionResult<IEnumerable<Repuesto>>> GetByProveedor(long proveedorId)
        {
            var list = await _ctx.Repuestos
                .Where(r => r.IdProveedor == proveedorId)
                .ToListAsync();
            return Ok(list);
        }

        // POST api/Repuestos
        [HttpPost]
        public async Task<ActionResult<Repuesto>> Create(RepuestoCreateDto dto)
        {
            if (dto.IdProveedor.HasValue &&
                !await _ctx.Proveedores.AnyAsync(p => p.Id == dto.IdProveedor.Value))
                return BadRequest($"Proveedor {dto.IdProveedor.Value} no existe.");

            var entity = new Repuesto
            {
                Nombre = dto.Nombre,
                NumeroParte = dto.NumeroParte,
                Descripcion = dto.Descripcion,
                CostoCompra = dto.CostoCompra,
                PrecioVenta = dto.PrecioVenta,
                CantidadStock = dto.CantidadStock,
                IdProveedor = dto.IdProveedor
            };

            _ctx.Repuestos.Add(entity);
            await _ctx.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }

        // PUT api/Repuestos/5
        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, Repuesto dto)
        {
            if (id != dto.Id) return BadRequest("Id de ruta y del cuerpo no coinciden.");

            if (dto.IdProveedor.HasValue)
            {
                var exists = await _ctx.Proveedores.AnyAsync(p => p.Id == dto.IdProveedor.Value);
                if (!exists) return BadRequest($"Proveedor {dto.IdProveedor.Value} no existe.");
            }

            _ctx.Entry(dto).State = EntityState.Modified;
            await _ctx.SaveChangesAsync();
            return NoContent();
        }

        // DELETE api/Repuestos/5
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var entity = await _ctx.Repuestos.FindAsync(id);
            if (entity == null) return NotFound();

            _ctx.Repuestos.Remove(entity);
            await _ctx.SaveChangesAsync();
            return NoContent();
        }
    }
}

