namespace Product.Infrastructure;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public class ProductContext : DbContext, IUnitOfWork
{
    public ProductContext(DbContextOptions options) : base(options) 
    { 
    
    }

    public DbSet<ProductEntity> Products { get; set; }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("dbo");

        modelBuilder.ApplyConfiguration(new ProductEntityConfiguration());

        foreach (var property in modelBuilder
            .Model
            .GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetColumnType("decimal(28,10)"); // Maximum SQL Server > 2012 is 38 Digit
        }
    }
}