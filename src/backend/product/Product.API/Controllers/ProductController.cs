using MediatR;
using Product.Application.Commands.ProductCommand;

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

    [HttpPost]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<int>> CreateProduct([FromBody] CreateProductCommand command) => Ok(await _mediator.Send(command));

    [HttpPut]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> UpdateProduct([FromBody] UpdateProductCommand command) => Ok(await _mediator.Send(command));

}