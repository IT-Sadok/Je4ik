using DocsAndHospitals.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class AuthService
{
    private readonly AuthRepository _repo;
    private readonly PasswordHasher _hasher;
    private readonly IConfiguration _configuration;

    public AuthService(AuthRepository repo, PasswordHasher hasher, IConfiguration configuration)
    {
        _repo = repo;
        _hasher = hasher;
        _configuration = configuration;
    }

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        if (await _repo.GetByEmailAsync(request.Email) != null)
            return false;

        var user = new User
        {
            Email = request.Email,
            PasswordHash = _hasher.Hash(request.Password),
            Role = request.Role
        };

        await _repo.AddUserAsync(user);
        return true;
    }

    public async Task<string?> LoginAsync(LoginRequest request)
    {
        var user = await _repo.GetByEmailAsync(request.Email);
        if (user == null || !_hasher.Verify(user.PasswordHash, request.Password))
            return null;

        return GenerateJwtToken(user);
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim("id", user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
