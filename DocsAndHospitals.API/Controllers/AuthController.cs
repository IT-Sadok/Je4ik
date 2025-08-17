using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using FluentValidation.Results;
using System.Threading.Tasks;
using DocsAndHospitals.Application.DTOs;
using DocsAndHospitals.Application.Interfaces;

namespace DocsAndHospitals.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IValidator<RegisterRequest> _registerValidator;
        private readonly IValidator<LoginRequest> _loginValidator;

        public AuthController(
            IAuthService authService,
            IValidator<RegisterRequest> registerValidator,
            IValidator<LoginRequest> loginValidator)
        {
            _authService = authService;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            ValidationResult validationResult = await _registerValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                return BadRequest(new { Errors = errors });
            }

            var success = await _authService.RegisterAsync(request);
            if (!success)
                return Conflict(new { Message = "User with this email already exists." });

            return Ok(new { Message = "User registered successfully." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            ValidationResult validationResult = await _loginValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                return BadRequest(new { Errors = errors });
            }

            var token = await _authService.LoginAsync(request);
            if (token == null)
                return Unauthorized(new { Message = "Invalid email or password." });

            return Ok(new { Token = token });
        }
    }
}
