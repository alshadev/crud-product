namespace Identity.API.Infrastructure.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User> GetByIdAsync(Guid id);
    Task<User> GetByUserNameAsync(string username);
}

public class UserRepository: BaseRepository<IdentityContext, User>, IUserRepository
{
    public UserRepository(IdentityContext context) 
        : base(context)
    {
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        var user = await Entities
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception("User by Id not found");

        return user;
    }

    public async Task<User> GetByUserNameAsync(string username)
    {
         var user = await Entities
            .FirstOrDefaultAsync(x => x.Username == username);

        return user;
    }
}