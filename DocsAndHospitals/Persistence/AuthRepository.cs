using DocsAndHospitals.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class AuthRepository
{
    private readonly List<User> _users = new();

    public AuthRepository()
    {
        var hasher = new PasswordHasher();
        string hashedPassword = hasher.Hash("111");

        _users.Add(new User
        {
            Id = 1,
            Email = "user",
            PasswordHash = hashedPassword,
            Role = Role.Client
        });
    }

    public Task AddUserAsync(User user)
    {
        _users.Add(user);
        return Task.CompletedTask;
    }

    public Task<User?> GetByEmailAsync(string email)
    {
        var user = _users.FirstOrDefault(x => x.Email == email);
        return Task.FromResult(user);
    }
}
