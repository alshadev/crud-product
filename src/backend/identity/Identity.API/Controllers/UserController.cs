namespace Identity.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class UserController : ControllerBase
{

    private readonly IMediator _mediator;
    
    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("Ping")]
    public IActionResult Ping()
    {
        return Ok();
    }

    [HttpPost("Registers")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Guid>> CreateProduct([FromBody] RegisterUserCommand command) => Ok(await _mediator.Send(command));
}