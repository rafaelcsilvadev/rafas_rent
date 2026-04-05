using FluentValidation;

namespace Auth.API.Dtos;

public record ForgotPasswordDto(string Email);

public class ForgotPasswordDtoValidator : AbstractValidator<ForgotPasswordDto>
{
    public ForgotPasswordDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Must be a valid email format");
    }
}