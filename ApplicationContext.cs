using Microsoft.EntityFrameworkCore;

public class ApplicationContext : DbContext
{
    public required DbSet<User> User { get; set; }

    public required DbSet<TipoPersona> TipoPersona { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=miBaseDeDatos.db");
    }
}