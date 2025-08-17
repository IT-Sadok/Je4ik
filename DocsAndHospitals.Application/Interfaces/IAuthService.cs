using DocsAndHospitals.Application.DTOs;
namespace DocsAndHospitals.Application.Interfaces
{ 
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterRequest request);
        Task<string?> LoginAsync(LoginRequest request);
    }
}


