using FluentValidation;
using DocsAndHospitals.Models;

public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email is required and must be a valid email address.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
    }
}