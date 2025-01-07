namespace Product.Infrastructure.EntityConfigurations;

public class ProductEntityConfiguration : IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.Property(x => x.Name).IsRequired();

        builder.HasIndex(x => x.Name);
        builder.HasIndex(x => x.Price);
        builder.HasIndex(x => x.CreatedAt);
    }
}