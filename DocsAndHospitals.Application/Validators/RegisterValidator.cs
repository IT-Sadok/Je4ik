using FluentValidation;
using DocsAndHospitals.Application.DTOs;        

namespace DocsAndHospitals.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be a valid email address.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage("Confirm password must match the password.");

            RuleFor(x => x.Role)
                .IsInEnum()
                .WithMessage("Role must be a valid value.");
        }
    }
}
