using Amazon.CognitoIdentityProvider.Model;

namespace Auth.API.Models;

public class BaseModel
{
    public string AccessToken { get; set; } = string.Empty;
    public int ExpiresIn { get; set; } = 0;
    public string IdToken { get; set; } = string.Empty;
    public string? RefreshToken { get; set; } = string.Empty;
    public string TokenType { get; set; } = string.Empty;

    public static implicit operator BaseModel(AdminInitiateAuthResponse response)
    {
        return new BaseModel
        {
            AccessToken = response.AuthenticationResult.AccessToken ?? string.Empty,
            ExpiresIn = response.AuthenticationResult.ExpiresIn ?? 0,
            IdToken = response.AuthenticationResult.IdToken ?? string.Empty,
            RefreshToken = response.AuthenticationResult.RefreshToken ?? string.Empty,
            TokenType = response.AuthenticationResult.TokenType ?? string.Empty
        };
    }

    public static implicit operator BaseModel(AdminRespondToAuthChallengeResponse response)
    {
        return new BaseModel
        {
            AccessToken = response.AuthenticationResult?.AccessToken ?? string.Empty,
            ExpiresIn = response.AuthenticationResult?.ExpiresIn ?? 0,
            IdToken = response.AuthenticationResult?.IdToken ?? string.Empty,
            RefreshToken = response.AuthenticationResult?.RefreshToken ?? string.Empty,
            TokenType = response.AuthenticationResult?.TokenType ?? string.Empty
        };
    }
}