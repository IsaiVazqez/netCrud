using MiApi.Dtos;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IActionResult> GetAllUsers([FromQuery] int pageSize, [FromQuery] int pageNumber = 1)
        {
            var users = await _context.User.Include(u => u.TipoPersona).ToListAsync();

            var userDtos = users.Select(u => new UserDTO
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Ciudad = u.Ciudad,
                Estado = u.Estado,
                TipoPersona = new TipoPersonaDTO
                {
                    Id = u.TipoPersonaId,
                    Nombre = u.TipoPersona?.Nombre
                },
                TipoPersonaNombre = u.TipoPersona?.Nombre
            }).ToList();

            // Contar el total de registros y calcular el total de páginas
            int totalRecords = userDtos.Count;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            // Paginar los resultados
            var paginatedUserDtos = userDtos.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var response = new
            {
                data = paginatedUserDtos,
                total = totalRecords,
                pageSize = pageSize,
                pageNumber = pageNumber
            };

            Response.Headers.Add("X-Total-Count", totalRecords.ToString());
            Response.Headers.Add("X-Total-Pages", totalPages.ToString());

            return Ok(response);
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
            Console.WriteLine($"TipoPersonaId recibido: {updateUserDTO.TipoPersonaId}");

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