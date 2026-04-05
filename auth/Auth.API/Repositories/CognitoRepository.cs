using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Auth.API.Models;

namespace Auth.API.Repositories;

public interface ICognitoRepository<T>
    where T : BaseModel
{
    Task<T> SignIn(string email, string password);
    Task ForgotPassword(string email);
    Task ConfirmForgotPassword(string email, string code, string password);
    Task SignUp(string email, string password);
    Task ConfirmSignUp(string email, string code);
    Task<T> FirstAccess(string email, string code, string name, string password);
    Task VerifyEmail(string email, string code);
    Task<T> RefreshToken(string refreshToken);
    Task UpdateUser(string email, string name);
    Task DeleteUser(string email);
}

public class CognitoRepository<T> : ICognitoRepository<T>
    where T : BaseModel
{
    private readonly AmazonCognitoIdentityProviderClient _cognitoClient;
    private readonly string _userPoolId;
    private readonly string _clientId;
    private readonly ILogger<CognitoRepository<T>> _logger;
    private readonly T _model;

    public CognitoRepository(
        AmazonCognitoIdentityProviderClient cognitoClient,
        ILogger<CognitoRepository<T>> logger
    )
    {
        _cognitoClient = cognitoClient;
        _userPoolId = Environment.GetEnvironmentVariable("AWS_COGNITO_USER_POOL_ID") ?? "";
        _clientId = Environment.GetEnvironmentVariable("AWS_COGNITO_CLIENT_ID") ?? "";
        _logger = logger;
        _model = Activator.CreateInstance<T>();
    }

    private bool CheckSignInVariables()
    {
        return !string.IsNullOrEmpty(_userPoolId) && !string.IsNullOrEmpty(_clientId);
    }

    public async Task<T> SignIn(string email, string password)
    {
        if (!CheckSignInVariables())
        {
            _logger.LogError("AWS_COGNITO_USER_POOL_ID or AWS_COGNITO_CLIENT_ID is not set");
            throw new Exception("AWS_COGNITO_USER_POOL_ID or AWS_COGNITO_CLIENT_ID is not set");
        }

        var authRequest = new AdminInitiateAuthRequest
        {
            UserPoolId = _userPoolId,
            ClientId = _clientId,
            AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH,
            AuthParameters = new Dictionary<string, string>
            {
                { "USERNAME", email },
                { "PASSWORD", password },
            },
        };

        var response = await _cognitoClient.AdminInitiateAuthAsync(authRequest);

        return (T)(BaseModel)response;
    }

    private bool CheckForgotPasswordVariables()
    {
        return !string.IsNullOrEmpty(_clientId);
    }

    public async Task ForgotPassword(string email)
    {
        if (!CheckForgotPasswordVariables())
        {
            _logger.LogError("AWS_COGNITO_CLIENT_ID is not set");
            throw new Exception("AWS_COGNITO_CLIENT_ID is not set");
        }

        var forgotPasswordRequest = new ForgotPasswordRequest
        {
            ClientId = _clientId,
            Username = email,
        };

        await _cognitoClient.ForgotPasswordAsync(forgotPasswordRequest);
    }

    private bool CheckConfirmForgotPasswordVariables()
    {
        return !string.IsNullOrEmpty(_clientId);
    }

    public async Task ConfirmForgotPassword(string email, string code, string password)
    {
        if (!CheckConfirmForgotPasswordVariables())
        {
            _logger.LogError("AWS_COGNITO_CLIENT_ID is not set");
            throw new Exception("AWS_COGNITO_CLIENT_ID is not set");
        }

        var confirmForgotPasswordRequest = new ConfirmForgotPasswordRequest
        {
            ClientId = _clientId,
            Username = email,
            ConfirmationCode = code,
            Password = password,
        };

        await _cognitoClient.ConfirmForgotPasswordAsync(confirmForgotPasswordRequest);
    }

    private bool CheckSignUpVariables()
    {
        return !string.IsNullOrEmpty(_userPoolId) && !string.IsNullOrEmpty(_clientId);
    }

    public async Task SignUp(string email, string password)
    {
        if (!CheckSignUpVariables())
        {
            _logger.LogError("AWS_COGNITO_USER_POOL_ID or AWS_COGNITO_CLIENT_ID is not set");
            throw new Exception("AWS_COGNITO_USER_POOL_ID or AWS_COGNITO_CLIENT_ID is not set");
        }

        var signUpRequest = new AdminCreateUserRequest
        {
            Username = email,
            UserPoolId = _userPoolId,
            UserAttributes = new List<AttributeType>
            {
                new AttributeType { Name = "email", Value = email },
            },
        };

        await _cognitoClient.AdminCreateUserAsync(signUpRequest);
    }

    private bool CheckConfirmSignUpVariables()
    {
        return !string.IsNullOrEmpty(_clientId);
    }

    public async Task ConfirmSignUp(string email, string code)
    {
        if (!CheckConfirmSignUpVariables())
        {
            _logger.LogError("AWS_COGNITO_CLIENT_ID is not set");
            throw new Exception("AWS_COGNITO_CLIENT_ID is not set");
        }

        var confirmSignUpRequest = new ConfirmSignUpRequest
        {
            ClientId = _clientId,
            Username = email,
            ConfirmationCode = code,
        };

        await _cognitoClient.ConfirmSignUpAsync(confirmSignUpRequest);
    }

    private bool CheckFirstAccessVariables()
    {
        return !string.IsNullOrEmpty(_clientId);
    }

    public async Task<T> FirstAccess(string email, string code, string name, string password)
    {
        if (!CheckFirstAccessVariables())
        {
            _logger.LogError("AWS_COGNITO_CLIENT_ID is not set");
            throw new Exception("AWS_COGNITO_CLIENT_ID is not set");
        }

        var firstAccessRequest = new AdminRespondToAuthChallengeRequest
        {
            ClientId = _clientId,
            UserPoolId = _userPoolId,
            ChallengeName = ChallengeNameType.NEW_PASSWORD_REQUIRED,
            Session = code,
            ChallengeResponses = new Dictionary<string, string>
            {
                { "USERNAME", email },
                { "NEW_PASSWORD", password },
                { "userAttributes.name", name },
            },
        };

        var response = await _cognitoClient.AdminRespondToAuthChallengeAsync(firstAccessRequest);

        return (T)(BaseModel)response;
    }

    private bool CheckVerifyEmailVariables()
    {
        return !string.IsNullOrEmpty(_userPoolId) && !string.IsNullOrEmpty(_clientId);
    }

    public async Task VerifyEmail(string email, string code)
    {
        if (!CheckVerifyEmailVariables())
        {
            _logger.LogError("AWS_COGNITO_USER_POOL_ID or AWS_COGNITO_CLIENT_ID is not set");
            throw new Exception("AWS_COGNITO_USER_POOL_ID or AWS_COGNITO_CLIENT_ID is not set");
        }

        var verifyEmailRequest = new AdminUpdateUserAttributesRequest
        {
            UserPoolId = _userPoolId,
            Username = email,
            UserAttributes = new List<AttributeType>
                {
                    new AttributeType { Name = "email_verified", Value = "true" },
                },
        };

        await _cognitoClient.AdminUpdateUserAttributesAsync(verifyEmailRequest);

        return;
    }

    private bool CheckUpdateUserVariables()
    {
        return !string.IsNullOrEmpty(_userPoolId);
    }

    public async Task UpdateUser(string email, string name)
    {
        if (!CheckUpdateUserVariables())
        {
            _logger.LogError("AWS_COGNITO_USER_POOL_ID is not set");
            throw new Exception("AWS_COGNITO_CLIENT_ID is not set");
        }

        var updateUserRequest = new AdminUpdateUserAttributesRequest
        {
            UserPoolId = _userPoolId,
            Username = email,
            UserAttributes = new List<AttributeType>
                {
                    new AttributeType { Name = "name", Value = name },
                },
        };

        await _cognitoClient.AdminUpdateUserAttributesAsync(updateUserRequest);

        return;
    }

    private bool CheckDeleteUserVariables()
    {
        return !string.IsNullOrEmpty(_userPoolId);
    }

    public async Task DeleteUser(string email)
    {
        if (!CheckDeleteUserVariables())
        {
            _logger.LogError("AWS_COGNITO_USER_POOL_ID is not set");
            throw new Exception("AWS_COGNITO_USER_POOL_ID is not set");
        }

        var deleteUserRequest = new AdminDeleteUserRequest
        {
            UserPoolId = _userPoolId,
            Username = email,
        };

        await _cognitoClient.AdminDeleteUserAsync(deleteUserRequest);

        return;
    }

    private bool CheckRefreshTokenVariables()
    {
        return !string.IsNullOrEmpty(_clientId) && !string.IsNullOrEmpty(_userPoolId);
    }

    public async Task<T> RefreshToken(string refreshToken)
    {
        if (!CheckRefreshTokenVariables())
        {
            _logger.LogError("AWS_COGNITO_CLIENT_ID is not set");
            throw new Exception("AWS_COGNITO_CLIENT_ID is not set");
        }


        var refreshTokenRequest = new AdminInitiateAuthRequest
        {
            ClientId = _clientId,
            UserPoolId = _userPoolId,
            AuthFlow = AuthFlowType.REFRESH_TOKEN_AUTH,
            AuthParameters = new Dictionary<string, string>
                {
                    { "REFRESH_TOKEN", refreshToken },
                },
        };

        var response = await _cognitoClient.AdminInitiateAuthAsync(refreshTokenRequest);

        return (T)(BaseModel)response;
    }
}
