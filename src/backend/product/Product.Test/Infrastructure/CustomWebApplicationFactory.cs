namespace Product.Test.Infrastructure;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing DbContext registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ProductContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            // Add the in-memory database for testing
            services.AddDbContext<ProductContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });
        });
    }
}
