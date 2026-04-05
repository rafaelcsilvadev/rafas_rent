namespace Auth.API.Dtos;

public class ConfirmForgotPasswordDto
{
    public string Email { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}