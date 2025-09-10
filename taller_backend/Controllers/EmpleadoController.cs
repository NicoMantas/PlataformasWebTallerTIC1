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
    public class EmpleadoController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        public EmpleadoController(ApplicationDbContext ctx) { _ctx = ctx; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetAll()
            => Ok(await _ctx.Empleados.ToListAsync());

        [HttpGet("{id:long}")]
        public async Task<ActionResult<Empleado>> GetById(long id)
            => await _ctx.Empleados.FindAsync(id) is { } e ? Ok(e) : NotFound();

        [HttpPost]
        public async Task<ActionResult<Empleado>> Create(EmpleadoCreateDto dto)
        {
            if (dto.IdDetalleTipoPersona.HasValue &&
                !await _ctx.DetallesTipoPersona.AnyAsync(x => x.Id == dto.IdDetalleTipoPersona.Value))
                return BadRequest($"DetalleTipoPersona {dto.IdDetalleTipoPersona} no existe.");

            var e = new Empleado
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Cedula = dto.Cedula ?? 0,
                Salario = dto.Salario,
                FechaContratacion = dto.FechaContratacion,
                IdDetalleTipoPersona = dto.IdDetalleTipoPersona ?? 0
            };

            _ctx.Empleados.Add(e);
            await _ctx.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = e.Id }, e);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, EmpleadoUpdateDto dto)
        {
            var e = await _ctx.Empleados.FindAsync(id);
            if (e == null) return NotFound();

            if (dto.IdDetalleTipoPersona.HasValue &&
                !await _ctx.DetallesTipoPersona.AnyAsync(x => x.Id == dto.IdDetalleTipoPersona.Value))
                return BadRequest($"DetalleTipoPersona {dto.IdDetalleTipoPersona} no existe.");

            e.Nombre = dto.Nombre;
            e.Apellido = dto.Apellido;
            e.Cedula = dto.Cedula ?? 0;
            e.Salario = dto.Salario;
            e.FechaContratacion = dto.FechaContratacion;
            e.IdDetalleTipoPersona = dto.IdDetalleTipoPersona ?? 0;

            await _ctx.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var e = await _ctx.Empleados.FindAsync(id);
            if (e == null) return NotFound();
            _ctx.Empleados.Remove(e);
            await _ctx.SaveChangesAsync();
            return NoContent();
        }
    }
}