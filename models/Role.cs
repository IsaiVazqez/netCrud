public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }  // Ejemplo: "Admin", "User", etc.
    public ICollection<User> Users { get; set; } = new List<User>();
}
