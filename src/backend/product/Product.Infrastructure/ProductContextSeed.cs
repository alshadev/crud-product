namespace Product.Infrastructure;

public interface IDbSeeder<in TContext> where TContext : DbContext
{
    Task SeedAsync(TContext context);
}

public class ProductContextSeed : IDbSeeder<ProductContext>
{
    public async Task SeedAsync(ProductContext context)
    {
        if (!await context.Products.AnyAsync()) 
        {
            var product1 = new ProductEntity("product1", 50);

            var products = new List<ProductEntity>() 
            { 
                product1
            };

            await context.AddRangeAsync(products);
        }
        await context.SaveChangesAsync();
    }
}