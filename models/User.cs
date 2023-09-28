
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("User")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? Name { get; set; }  
    public string? Email { get; set; }  
    public string? Ciudad { get; set; } 
    public string? Estado { get; set; } 
    public int TipoPersonaId { get; set; }


    
}
