namespace Auth.API.Dtos;

public class ConfirmSignUpDto
{
    public string Email { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}
