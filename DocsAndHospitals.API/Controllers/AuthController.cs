using Microsoft.AspNetCore.Mvc;
using DocsAndHospitals.Models;
using DocsAndHospitals.Services; // де твій AuthService
using FluentValidation;
using FluentValidation.Results;

namespace DocsAndHospitals.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IValidator<RegisterRequest> _registerValidator;
        private readonly IValidator<LoginRequest> _loginValidator;

        public AuthController(AuthService authService,
            IValidator<RegisterRequest> registerValidator,
            IValidator<LoginRequest> loginValidator)
        {
            _authService = authService;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            ValidationResult result = _registerValidator.Validate(request);
            if (!result.IsValid)
                return BadRequest(result.Errors);

            bool success = _authService.RegisterAsync(request).Result;
            if (!success)
                return Conflict("User with this email already exists.");

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            ValidationResult result = _loginValidator.Validate(request);
            if (!result.IsValid)
                return BadRequest(result.Errors);

            var user = _authService.LoginAsync(request);
            if (user == null)
                return Unauthorized("Invalid email or password.");
            return Ok(user);
        }
    }
}
