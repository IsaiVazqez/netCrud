using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MiApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TipoPersonaController : ControllerBase
    {
        private readonly ApplicationContext _context; 

        public TipoPersonaController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoPersona>>> GetAllTipoPersonas()
        {
            return await _context.TipoPersona.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TipoPersona>> GetTipoPersona(int id)
        {
            var tipoPersona = await _context.TipoPersona.FindAsync(id);
            
            if (tipoPersona == null)
            {
                return NotFound();
            }
            return tipoPersona;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTipoPersona(int id, TipoPersona tipoPersona)
        {
            if (id != tipoPersona.Id)
            {
                return BadRequest("El ID proporcionado no coincide con el ID del objeto.");
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
        public async Task<ActionResult<TipoPersona>> CreateTipoPersona(TipoPersona tipoPersona)
        {
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
                return BadRequest("Este tipo de persona ya está asignado a un usuario y no puede ser eliminado.");
            }

            _context.TipoPersona.Remove(tipoPersona);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}