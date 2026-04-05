using FluentValidation;

namespace Auth.API.Dtos;

public record DeleteUserDto(string Email);

public class DeleteUserDtoValidator : AbstractValidator<DeleteUserDto>
{
    public DeleteUserDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Must be a valid email format");
    }
}