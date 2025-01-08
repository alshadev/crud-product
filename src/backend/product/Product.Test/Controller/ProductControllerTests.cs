namespace Product.Test.Controller;

public class ProductControllerTests
{
    private readonly ProductController _controller;
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<ILogger<HttpGlobalExceptionFilter>> _logger;

    public ProductControllerTests()
    {
        _mediator = new Mock<IMediator>();
        _controller = new ProductController(_mediator.Object);
        _logger = new Mock<ILogger<HttpGlobalExceptionFilter>>();
    }

    [Fact]
    public async Task CreateProducts_ReturnOk()
    {
        // Arrange
        var command = new CreateProductCommand { Name = "Product 1", Price = 100, Description = "Description 1" };
        var result = 1;

        _mediator.Setup(m => m.Send(It.IsAny<CreateProductCommand>(), default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.CreateProduct(command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result); 
        var actualValue = Assert.IsType<int>(okResult.Value); 
        Assert.Equal(1, actualValue);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task CreateProducts_ReturnBadRequest()
    {
        // Arrange
        var command = new CreateProductCommand { Name = "Product 1", Price = 100, Description = "Description 1" };

        _mediator.Setup(m => m.Send(It.IsAny<CreateProductCommand>(), default))
            .ThrowsAsync(new Exception("create product failed"));

        var actionContext = new ActionContext(
            new DefaultHttpContext(), 
            new Microsoft.AspNetCore.Routing.RouteData(), 
            new ControllerActionDescriptor()
        );

        var exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>());

        // Act
        Exception caughtException = null;
        try
        {
            var actionResult = await _controller.CreateProduct(command); 
        }
        catch (Exception ex)
        {
            caughtException = ex;
        }

        exceptionContext.Exception = caughtException;

        var exceptionFilter = new HttpGlobalExceptionFilter(_logger.Object); 
        exceptionFilter.OnException(exceptionContext);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(exceptionContext.Result);  
        var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value); 
        Assert.Equal((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
        Assert.Contains("Exceptions", problemDetails.Errors.Keys); 
        Assert.Contains("create product failed", problemDetails.Errors["Exceptions"].First());
    }
}
