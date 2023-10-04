using Microsoft.EntityFrameworkCore;

public class ApplicationContext : DbContext
{
    public required DbSet<User> User { get; set; }
    public DbSet<Role> Role { get; set; }
    public required DbSet<TipoPersona> TipoPersona { get; set; }
    public DbSet<Product> Product { get; set; }

    public DbSet<Midier> Midier { get; set; }

    public DbSet<Order> Order { get; set; }

    public DbSet<OrderProduct> OrderProduct { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=miBaseDeDatos.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Image)
            .WithMany()
            .HasForeignKey(p => p.IdImage);

        modelBuilder.Entity<OrderProduct>()
.HasKey(op => new { op.OrderId, op.ProductId }); // Clave compuesta

        modelBuilder.Entity<OrderProduct>()
            .HasOne(op => op.Order)
            .WithMany(o => o.OrderProducts)
            .HasForeignKey(op => op.OrderId);

        modelBuilder.Entity<OrderProduct>()
            .HasOne(op => op.Product)
            .WithMany(p => p.OrderProducts)
            .HasForeignKey(op => op.ProductId);
    }
}