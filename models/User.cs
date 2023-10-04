
public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Ciudad { get; set; }
    public string? Estado { get; set; }
    public int TipoPersonaId { get; set; }
    public TipoPersona? TipoPersona { get; set; }
    public int RoleId { get; set; }
    public Role? Role { get; set; }
    public string? Password { get; set; }

}
