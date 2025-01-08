namespace Identity.API.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Username { get; private set; }
    public string Password { get; private set; }

    public User(string username, string password)
    {
        Username = string.IsNullOrWhiteSpace(username) ? throw new Exception("Username is mandatory") : username.Trim();
        Password = string.IsNullOrWhiteSpace(password) ? throw new Exception("Password is mandatory") : password;
    }
}
