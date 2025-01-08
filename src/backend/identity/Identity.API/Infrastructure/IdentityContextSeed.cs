using System;

namespace Identity.API.Infrastructure;

public interface IDbSeeder<in TContext> where TContext : DbContext
{
    Task SeedAsync(TContext context);
}

public class IdentityContextSeed: IDbSeeder<IdentityContext>
{
    public async Task SeedAsync(IdentityContext context)
    {
        if (!await context.Users.AnyAsync()) 
        {
            var user1 = new User("admin", "admin123");

            var users = new List<User>() 
            { 
                user1
            };

            await context.AddRangeAsync(users);
        }
        await context.SaveChangesAsync();
    }
}