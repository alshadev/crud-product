namespace Identity.API.Application.Commands.UserCommand;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    public LoginUserCommandHandler(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUsernamePasswordAsync(request.Username, request.Password);

        if (user == null)
        {
            return null;
        }

        return _tokenService.GenerateToken(user.Username);
    }
}
