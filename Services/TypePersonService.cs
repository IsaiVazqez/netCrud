using AutoMapper;
using MiApi.Dtos;
using Microsoft.EntityFrameworkCore;

public class TypePersonService
{
    private readonly ApplicationContext _context;
    private readonly IMapper _mapper;

    public TypePersonService(ApplicationContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<TipoPersonaDTO>> GetAllTipoPersonasAsync()
    {
        var tipoPersonas = await _context.TipoPersona.ToListAsync();
        return _mapper.Map<List<TipoPersonaDTO>>(tipoPersonas);
    }

    public async Task<TipoPersonaDTO?> GetTipoPersonaAsync(int id)
    {
        var tipoPersona = await _context.TipoPersona.FindAsync(id);
        return tipoPersona != null ? _mapper.Map<TipoPersonaDTO>(tipoPersona) : null;
    }

    public async Task<TipoPersonaDTO?> UpdateTipoPersonaAsync(int id, UpdateTipoPersonaDTO updateTipoPersonaDto)
    {
        var tipoPersona = await _context.TipoPersona.FindAsync(id);
        if (tipoPersona == null)
        {
            return null;
        }

        _mapper.Map(updateTipoPersonaDto, tipoPersona);
        _context.Entry(tipoPersona).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return _mapper.Map<TipoPersonaDTO>(tipoPersona);
    }

    public async Task<TipoPersonaDTO> CreateTipoPersonaAsync(TipoPersonaDTO tipoPersonaDto)
    {
        var tipoPersona = _mapper.Map<TipoPersona>(tipoPersonaDto);
        _context.TipoPersona.Add(tipoPersona);
        await _context.SaveChangesAsync();
        return _mapper.Map<TipoPersonaDTO>(tipoPersona);
    }

    public async Task<bool> DeleteTipoPersonaAsync(int id)
    {
        var tipoPersona = await _context.TipoPersona.FindAsync(id);
        if (tipoPersona == null)
        {
            return false;
        }

        if (_context.User.Any(u => u.TipoPersonaId == id))
        {
            throw new InvalidOperationException("Este tipo de persona ya está asignado a un usuario y no puede ser eliminado.");
        }

        _context.TipoPersona.Remove(tipoPersona);
        await _context.SaveChangesAsync();
        return true;
    }
}
