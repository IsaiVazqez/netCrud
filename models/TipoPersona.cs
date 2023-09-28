public class TipoPersona
{
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public ICollection<User>? User { get; set; }  // Esto indica que varios usuarios pueden tener el mismo TipoPersona
   
}
