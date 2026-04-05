using FluentValidation;

namespace Auth.API.Dtos;

public record ConfirmForgotPasswordDto(string Email, string Code, string Password);

public class ConfirmForgotPasswordDtoValidator : AbstractValidator<ConfirmForgotPasswordDto>
{
    public ConfirmForgotPasswordDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Must be a valid email format");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required")
            .MinimumLength(6).WithMessage("Code must be at least 6 characters");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters");
    }
}