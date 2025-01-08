namespace Product.Test.Functional;

public class ProductApiTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProductApiTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing"); 
        }).CreateClient();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");
    }

    [Fact]
    public async Task CreateProduct_ReturnsCreated()
    {
        // Arrange
        var requestUrl = "/api/v1/Product";
        var payload = new
        {
            Name = "Test Product",
            Price = 100m,
            Description = "Description"
        };
        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(payload), System.Text.Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync(requestUrl, content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
