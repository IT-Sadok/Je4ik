using Xunit;
using Shouldly;
using Moq;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using DocsAndHospitals.Domain;
using DocsAndHospitals.Persistence;
using DocsAndHospitals.Models;

public class AuthServiceTests
{
    [Fact]
    public async Task RegisterAsync_ShouldReturnFalse_IfUserAlreadyExists()
    {
        // Arrange
        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(r => r.GetByEmailAsync("test@example.com"))
            .ReturnsAsync(new User { Email = "test@example.com" });

        var hasher = new PasswordHasher();

        var inMemorySettings = new Dictionary<string, string> {
            {"JwtSettings:SecretKey", "some_secret_key_1234567890"},
            {"JwtSettings:Issuer", "TestIssuer"},
            {"JwtSettings:Audience", "TestAudience"}
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        var authService = new AuthService(mockRepo.Object, hasher, configuration);

        var request = new RegisterRequest
        {
            Email = "test@example.com",
            Password = "password",
            Role = Role.Client
        };

        // Act
        var result = await authService.RegisterAsync(request);

        // Assert
        result.ShouldBeFalse();
    }
}
