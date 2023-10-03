using MiApi.Dtos;
using OfficeOpenXml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace MiApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ApplicationContext _context;

        public UserController(ApplicationContext context, UserService userService)
        {
            _context = context;

            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] int pageSize, [FromQuery] int pageNumber = 1, [FromQuery] string orderDirection = "desc")
        {
            var userDtos = await _userService.GetAllUsersAsync(orderDirection);
            int totalRecords = userDtos.Count;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
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
            var userDTO = await _userService.GetUserByIdAsync(id);
            if (userDTO == null)
            {
                return NotFound();
            }

            return userDTO;
        }


        [HttpGet("ExportToExcel")]
        public async Task<IActionResult> ExportToExcel()
        {
            var users = await _context.User.Include(u => u.TipoPersona).ToListAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Users");

                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Email";
                worksheet.Cells[1, 4].Value = "Ciudad";
                worksheet.Cells[1, 5].Value = "Estado";
                worksheet.Cells[1, 6].Value = "Tipo de Persona";

                for (int i = 0; i < users.Count; i++)
                {
                    var user = users[i];
                    worksheet.Cells[i + 2, 1].Value = user.Id;
                    worksheet.Cells[i + 2, 2].Value = user.Name;
                    worksheet.Cells[i + 2, 3].Value = user.Email;
                    worksheet.Cells[i + 2, 4].Value = user.Ciudad;
                    worksheet.Cells[i + 2, 5].Value = user.Estado;
                    worksheet.Cells[i + 2, 6].Value = user.TipoPersona?.Nombre ?? "N/A";

                }

                var stream = new MemoryStream();
                package.SaveAs(stream);

                var content = stream.ToArray();

                return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Users.xlsx");
            }
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(CreateUserDTO createUserDTO)
        {
            var userDTO = await _userService.CreateUserAsync(createUserDTO);
            return CreatedAtAction("GetUser", new { id = userDTO.Id }, userDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UpdateUserDTO updateUserDTO)
        {
            var updatedUserDTO = await _userService.UpdateUserAsync(id, updateUserDTO);
            if (updatedUserDTO == null)
            {
                return NotFound();
            }
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }

}