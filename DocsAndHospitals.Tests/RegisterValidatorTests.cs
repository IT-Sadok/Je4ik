using Xunit;
using Shouldly;
using DocsAndHospitals.Models;
using DocsAndHospitals.Validators;
using DocsAndHospitals.Application.DTOs;    

namespace DocsAndHospitals.Application.Validators
{
    public class RegisterValidatorTests
    {
        private readonly RegisterValidator _validator = new RegisterValidator();

        [Fact]
        public void Should_Pass_When_RequestIsValid()
        {
            var request = new RegisterRequest
            {
                Email = "test@example.com",
                Password = "StrongPassword123",
                ConfirmPassword = "StrongPassword123",
                Role = Role.Client
            };

            var result = _validator.Validate(request);
            result.IsValid.ShouldBeTrue();
        }

        // інші тести...
    }
}
