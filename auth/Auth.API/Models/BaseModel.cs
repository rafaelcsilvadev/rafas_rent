namespace Auth.API.Models;

public class BaseModel
{
    public string AccessToken { get; set; } = string.Empty;
    public int ExpiresIn { get; set; } = 0;
    public string IdToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string TokenType { get; set; } = string.Empty;
}