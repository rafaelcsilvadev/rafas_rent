using FluentValidation;

namespace Auth.API.Dtos;

public record UpdateUserDto(string Email, string Name);

public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Must be a valid email format");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(3).WithMessage("Name must be at least 3 characters");
    }
}