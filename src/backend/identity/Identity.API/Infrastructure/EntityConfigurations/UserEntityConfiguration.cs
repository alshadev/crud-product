namespace Identity.API.Infrastructure.EntityConfigurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.Username).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Password).IsRequired();

        builder.HasIndex(x => x.Username).IsUnique();
    }
}