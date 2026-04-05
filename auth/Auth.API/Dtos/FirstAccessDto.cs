using FluentValidation;

namespace Auth.API.Dtos;

public record FirstAccessDto(string Email, string Code, string Password, string Name);

public class FirstAccessDtoValidator : AbstractValidator<FirstAccessDto>
{
    public FirstAccessDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Must be a valid email format");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required")
            .MinimumLength(6).WithMessage("Code must be at least 6 characters");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(3).WithMessage("Name must be at least 3 characters");
    }
}