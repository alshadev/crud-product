namespace Product.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    public ProductController()
    {
        
    }

    [HttpGet("Ping")]
    public IActionResult Ping()
    {
        return Ok();
    }
}