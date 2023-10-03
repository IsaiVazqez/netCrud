using MiApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace MiApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TypePersonController : ControllerBase
    {
        private readonly TypePersonService _tipoPersonaService;

        public TypePersonController(TypePersonService tipoPersonaService)
        {
            _tipoPersonaService = tipoPersonaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoPersonaDTO>>> GetAllTipoPersonas()
        {
            return Ok(await _tipoPersonaService.GetAllTipoPersonasAsync());
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<TipoPersonaDTO>> GetTipoPersona(int id)
        {
            var tipoPersonaDto = await _tipoPersonaService.GetTipoPersonaAsync(id);
            if (tipoPersonaDto == null)
            {
                return NotFound();
            }
            return Ok(tipoPersonaDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTipoPersona(int id, UpdateTipoPersonaDTO updateTipoPersonaDto)
        {
            var updatedTipoPersonaDto = await _tipoPersonaService.UpdateTipoPersonaAsync(id, updateTipoPersonaDto);
            if (updatedTipoPersonaDto == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<TipoPersonaDTO>> CreateTipoPersona(TipoPersonaDTO tipoPersonaDto)
        {
            var createdTipoPersonaDto = await _tipoPersonaService.CreateTipoPersonaAsync(tipoPersonaDto);
            return CreatedAtAction(nameof(GetTipoPersona), new { id = createdTipoPersonaDto.Id }, createdTipoPersonaDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoPersona(int id)
        {
            try
            {
                var success = await _tipoPersonaService.DeleteTipoPersonaAsync(id);
                if (!success)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}