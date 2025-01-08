using Microsoft.AspNetCore.Authorization;

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

    [HttpPost("Register")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Guid>> Register([FromBody] RegisterUserCommand command) => Ok(await _mediator.Send(command));

    [HttpPost("Login")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<string>> Login([FromBody] LoginUserCommand command) 
    {
        var token = await _mediator.Send(command);

        if (string.IsNullOrWhiteSpace(token))
        {
            return Unauthorized("Invalid credentials");
        }

        return Ok(token);
    } 
}