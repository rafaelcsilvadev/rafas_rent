using FluentValidation;

namespace Auth.API.Dtos;

public record VerifyEmailDto(string Email, string Code);

public class VerifyEmailDtoValidator : AbstractValidator<VerifyEmailDto>
{
    public VerifyEmailDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Must be a valid email format");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required")
            .MinimumLength(6).WithMessage("Code must be at least 6 characters");
    }
}