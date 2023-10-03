using AutoMapper;
using DocumentFormat.OpenXml.ExtendedProperties;
using MiApi.Dtos;
using Microsoft.EntityFrameworkCore;

public class UserService
{

    private readonly ApplicationContext _context;
    private readonly IMapper _mapper;

    public UserService(ApplicationContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<UserDTO>> GetAllUsersAsync(string orderDirection)
    {
        var users = await _context.User.Include(u => u.TipoPersona).ToListAsync();

        if (orderDirection.ToLower() == "asc")
        {
            users = users.OrderBy(u => u.Id).ToList();
        }
        else
        {
            users = users.OrderByDescending(u => u.Id).ToList();
        }

        return _mapper.Map<List<UserDTO>>(users);
    }


    public async Task<UserDTO> GetUserByIdAsync(int id)
    {
        var user = await _context.User.Include(u => u.TipoPersona).FirstOrDefaultAsync(u => u.Id == id);
        return _mapper.Map<UserDTO>(user);
    }


    public async Task<List<User>> ExportUsersToExcelAsync()
    {
        return await _context.User.Include(u => u.TipoPersona).ToListAsync();
    }

    public async Task<UserDTO> CreateUserAsync(CreateUserDTO createUserDTO)
    {
        var user = _mapper.Map<User>(createUserDTO);
        _context.User.Add(user);
        await _context.SaveChangesAsync();
        return _mapper.Map<UserDTO>(user);
    }

    public async Task<UserDTO> UpdateUserAsync(int id, UpdateUserDTO updateUserDTO)
    {
        var user = await _context.User.FindAsync(id);
        if (user == null)
        {
            return null;
        }

        _mapper.Map(updateUserDTO, user); // Esto mapeará las propiedades de updateUserDTO a user
        _context.Entry(user).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return _mapper.Map<UserDTO>(user);
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _context.User.FindAsync(id);
        if (user == null)
        {
            return false;
        }

        _context.User.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public bool UserExists(int id)
    {
        return _context.User.Any(e => e.Id == id);
    }

}