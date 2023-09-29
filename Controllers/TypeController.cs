using MiApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MiApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TypePersonController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public TypePersonController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoPersonaDTO>>> GetAllTipoPersonas()
        {
            var tipoPersonas = await _context.TipoPersona.ToListAsync();

            var tipoPersonasDto = tipoPersonas.Select(tp => new TipoPersonaDTO
            {
                Id = tp.Id,
                Nombre = tp.Nombre
            }).ToList();

            return Ok(tipoPersonasDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TipoPersonaDTO>> GetTipoPersona(int id)
        {
            var tipoPersona = await _context.TipoPersona.FindAsync(id);

            if (tipoPersona == null)
            {
                return NotFound();
            }

            var tipoPersonaDto = new TipoPersonaDTO
            {
                Id = tipoPersona.Id,
                Nombre = tipoPersona.Nombre
            };

            return Ok(tipoPersonaDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTipoPersona(int id, UpdateTipoPersona updateTipoPersonaDto)
        {
            var tipoPersona = await _context.TipoPersona.FindAsync(id);

            if (tipoPersona == null)
            {
                return NotFound();
            }

            if (updateTipoPersonaDto.Nombre != null)
            {
                tipoPersona.Nombre = updateTipoPersonaDto.Nombre;
            }

            _context.Entry(tipoPersona).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.TipoPersona.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<TipoPersonaDTO>> CreateTipoPersona(TipoPersonaDTO tipoPersonaDto)
        {
            var tipoPersona = new TipoPersona
            {
                Nombre = tipoPersonaDto.Nombre
            };

            _context.TipoPersona.Add(tipoPersona);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTipoPersona), new { id = tipoPersona.Id }, tipoPersona);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoPersona(int id)
        {
            var tipoPersona = await _context.TipoPersona.FindAsync(id);

            if (tipoPersona == null)
            {
                return NotFound();
            }

            if (_context.User.Any(u => u.TipoPersonaId == id))
            {
                return BadRequest(new { message = "Este tipo de persona ya está asignado a un usuario y no puede ser eliminado." });
            }

            _context.TipoPersona.Remove(tipoPersona);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}