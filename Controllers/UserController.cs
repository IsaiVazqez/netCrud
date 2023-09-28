using MiApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MiApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public UserController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await _context.User.Include(u => u.TipoPersona).ToListAsync();

            var userDTOs = users.Select(user => new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Ciudad = user.Ciudad,
                Estado = user.Estado,
                TipoPersona = new TipoPersonaDTO
                {
                    Id = user.TipoPersona.Id,
                    Nombre = user.TipoPersona.Nombre
                }
            }).ToList();

            return Ok(userDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await _context.User.Include(u => u.TipoPersona).FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            var userDTO = new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Ciudad = user.Ciudad,
                Estado = user.Estado,
                TipoPersona = new TipoPersonaDTO
                {
                    Id = user.TipoPersona.Id,
                    Nombre = user.TipoPersona.Nombre
                }
            };

            return userDTO;
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(CreateUserDto createUserDTO)
        {
            Console.WriteLine($"TipoPersonaId recibido: {createUserDTO.TipoPersonaId}");

            User user = new User
            {
                Name = createUserDTO.Name,
                Email = createUserDTO.Email,
                Ciudad = createUserDTO.Ciudad,
                Estado = createUserDTO.Estado,
                TipoPersonaId = createUserDTO.TipoPersonaId
            };

            TipoPersona tipoPersona = _context.TipoPersona.Find(user.TipoPersonaId);

            if (tipoPersona == null)
            {
                user.TipoPersonaId = tipoPersona?.Id ?? 0;
            }

            Console.WriteLine($"TipoPersonaId antes de guardar: {user.TipoPersonaId}");

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            UserDTO userDTO = new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Ciudad = user.Ciudad,
                Estado = user.Estado,
                TipoPersona = new TipoPersonaDTO
                {
                    Id = user.TipoPersonaId,
                    Nombre = _context.TipoPersona.FirstOrDefault(tp => tp.Id == user.TipoPersonaId)?.Nombre
                }
            };

            return CreatedAtAction("GetUser", new { id = userDTO.Id }, userDTO);
        }



        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UpdateUserDto updateUserDTO)
        {
            // Debugging: Verificar el valor de TipoPersonaId recibido
            Console.WriteLine($"TipoPersonaId recibido: {updateUserDTO.TipoPersonaId}");

            // Encuentra el usuario existente
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Name = updateUserDTO.Name;
            user.Email = updateUserDTO.Email;
            user.Ciudad = updateUserDTO.Ciudad;
            user.Estado = updateUserDTO.Estado;
            user.TipoPersonaId = updateUserDTO.TipoPersonaId;

            // Debugging: Verificar el valor de TipoPersonaId antes de guardar
            Console.WriteLine($"TipoPersonaId antes de guardar: {user.TipoPersonaId}");

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }

}