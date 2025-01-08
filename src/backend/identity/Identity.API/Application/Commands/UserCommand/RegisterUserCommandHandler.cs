namespace Identity.API.Application.Commands.UserCommand;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    public RegisterUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.GetByUserNameAsync(request.Username) != null)
        {
            throw new Exception("Username already exist");
        }

        var newUser = new User(request.Username, request.Password);

        await _userRepository.AddAsync(newUser);
        await _userRepository.UnitOfWork.SaveChangesAsync();

        return newUser.Id;
    }
}
