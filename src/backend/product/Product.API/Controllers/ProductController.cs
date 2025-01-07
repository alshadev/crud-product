using Product.Application.Queries.ProductQuery;

namespace Product.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("Ping")]
    public IActionResult Ping()
    {
        return Ok();
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ProductEntity>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetProducts([FromQuery] string name = null, [FromQuery] decimal? startPrice = null, [FromQuery] decimal? endPrice = null) 
    {
        var products = await _mediator.Send(new GetProductQuery()
        {
            Name = name,
            StartPrice = startPrice,
            EndPrice = endPrice
        });

        return Ok(products);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(List<ProductEntity>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetProductById(int id) 
    {
        var product = await _mediator.Send(new GetProductByIdQuery()
        {
            Id = id
        });

        return Ok(product);
    }

    [HttpPost]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<int>> CreateProduct([FromBody] CreateProductCommand command) => Ok(await _mediator.Send(command));

    [HttpPut]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> UpdateProduct([FromBody] UpdateProductCommand command) => Ok(await _mediator.Send(command));

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> DeleteProduct(int id) => Ok(await _mediator.Send(new DeleteProductCommand()
    {
        Id = id
    }));

}