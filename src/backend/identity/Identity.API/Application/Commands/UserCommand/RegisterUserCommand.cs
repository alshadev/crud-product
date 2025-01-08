namespace Identity.API.Application.Commands.UserCommand;

public class RegisterUserCommand : IRequest<Guid>
{
    [Required(ErrorMessage = "Username is mandatory")]
    public string Username { get; set; }
    public string Password { get; set; }
}
