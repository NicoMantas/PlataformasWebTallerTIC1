using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using taller_backend.ContextDB;
using taller_backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace taller_backend.Controllers
{
    [ApiController] // Indica que es un controlador de API
    [Route("api/[controller]")] // Ruta base para el controlador
    public class ProveedorController : Controller
    {
        private readonly ApplicationDbContext _ctx; // Contexto de la base de datos

        public ProveedorController(ApplicationDbContext ctx) // Constructor que recibe el contexto de la base de datos
        {
            _ctx = ctx;
        }

        [HttpGet] // Acción para obtener todos los proveedores
        public async Task<ActionResult<IEnumerable<Proveedor>>> GetAll()
        {
            var list = await _ctx.Proveedores.ToListAsync();
            return Ok(list);
        }

        [HttpGet("{id:long}")] // Acción para obtener un proveedor por su ID
        public async Task<ActionResult<Proveedor>> GetById(long id)
        {
            var entity = await _ctx.Proveedores.FindAsync(id);
            if (entity == null) return NotFound();
            return Ok(entity);
        }

        [HttpPost] // Acción para crear un nuevo proveedor
        public async Task<ActionResult<Proveedor>> Create(Proveedor dto)
        {
            _ctx.Proveedores.Add(dto);
            await _ctx.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id:long}")] // Acción para actualizar un proveedor existente
        public async Task<IActionResult> Update(long id, Proveedor dto)
        {
            if (id != dto.Id) return BadRequest("Id de ruta y cuerpo no coinciden.");

            _ctx.Entry(dto).State = EntityState.Modified;
            await _ctx.SaveChangesAsync();
            return NoContent();
        }
        
        [HttpDelete("{id:long}")] // Acción para eliminar un proveedor por su ID
        public async Task<IActionResult> Delete(long id)
        {
            var entity = await _ctx.Proveedores.FindAsync(id);
            if (entity == null) return NotFound();

            _ctx.Proveedores.Remove(entity);
            await _ctx.SaveChangesAsync();
            return NoContent();
        }

    }
}
