namespace MiApi.Dtos;
public class CreateUserDTO
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Ciudad { get; set; }
    public string? Estado { get; set; }
    public int TipoPersonaId { get; set; }
}
