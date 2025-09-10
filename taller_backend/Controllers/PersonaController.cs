using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using taller_backend.ContextDB;
using taller_backend.Models;
using taller_backend.DTOs;

namespace taller_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonaController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        public PersonaController(ApplicationDbContext ctx) { _ctx = ctx; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Persona>>> GetAll()
            => Ok(await _ctx.Personas.ToListAsync());

        [HttpGet("{id:long}")]
        public async Task<ActionResult<Persona>> GetById(long id)
            => await _ctx.Personas.FindAsync(id) is { } p ? Ok(p) : NotFound();

        [HttpPost]
        public async Task<ActionResult<Persona>> Create(PersonaCreateDto dto)
        {
            var p = new Persona { Nombre = dto.Nombre, Email = dto.Email, Telefono = dto.Telefono, IdTDetalleTipoPersona = dto.IdTDetalleTipoPersona };
            _ctx.Personas.Add(p);
            await _ctx.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = p.Id }, p);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, PersonaUpdateDto dto)
        {
            var p = await _ctx.Personas.FindAsync(id);
            if (p == null) return NotFound();
            p.Nombre = dto.Nombre;
            p.Email = dto.Email;
            p.Telefono = dto.Telefono;
            p.IdTDetalleTipoPersona = dto.IdTDetalleTipoPersona;
            await _ctx.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var p = await _ctx.Personas.FindAsync(id);
            if (p == null) return NotFound();
            _ctx.Personas.Remove(p);
            await _ctx.SaveChangesAsync();
            return NoContent();
        }
    }
}