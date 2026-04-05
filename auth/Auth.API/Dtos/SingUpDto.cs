using FluentValidation;

namespace Auth.API.Dtos;

public record SignUpDto(string Email, string Password);

public class SignUpDtoValidator : AbstractValidator<SignUpDto>
{
    public SignUpDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Must be a valid email format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters");
    }
}