using Amazon.CognitoIdentityProvider.Model;
using Amazon.Runtime;
using Auth.API.Dtos;
using Auth.API.Models;
using Auth.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ClienteController : ControllerBase
{
    private readonly ICognitoRepository<ClientModel> _cognitoRepository;
    private readonly ILogger<ClienteController> _logger;

    public ClienteController(
        ICognitoRepository<ClientModel> cognitoRepository,
        ILogger<ClienteController> logger
    )
    {
        _cognitoRepository = cognitoRepository;
        _logger = logger;
    }

    [HttpPost]
    [Route("sign-in")]
    public async Task<IActionResult> SignIn([FromBody] SignInDto signInDto)
    {
        try
        {
            var result = await _cognitoRepository.SignIn(signInDto.Email, signInDto.Password);
            return Ok(result);
        }
        catch (AmazonServiceException ex)
        {
            _logger.LogError("SignIn: {Message}", ex.Message);

            return StatusCode(
                (int)ex.StatusCode,
                new { StatusCode = ex.StatusCode, Message = ex.Message }
            );
        }
    }

    [HttpPost]
    [Route("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
    {
        try
        {
            await _cognitoRepository.ForgotPassword(forgotPasswordDto.Email);
            return Ok();
        }
        catch (AmazonServiceException ex)
        {
            _logger.LogError("ForgotPassword: {Message}", ex.Message);

            return StatusCode(
                (int)ex.StatusCode,
                new { StatusCode = ex.StatusCode, Message = ex.Message }
            );
        }
    }

    [HttpPost]
    [Route("confirm-forgot-password")]
    public async Task<IActionResult> ConfirmForgotPassword(
        [FromBody] ConfirmForgotPasswordDto confirmForgotPasswordDto
    )
    {
        try
        {
            await _cognitoRepository.ConfirmForgotPassword(
                confirmForgotPasswordDto.Email,
                confirmForgotPasswordDto.Code,
                confirmForgotPasswordDto.Password
            );
            return Ok();
        }
        catch (AmazonServiceException ex)
        {
            _logger.LogError("ConfirmForgotPassword: {Message}", ex.Message);

            return StatusCode(
                (int)ex.StatusCode,
                new { StatusCode = ex.StatusCode, Message = ex.Message }
            );
        }
    }

    [HttpPost]
    [Route("sign-up")]
    public async Task<IActionResult> SignUp([FromBody] SignUpDto signUpDto)
    {
        try
        {
            await _cognitoRepository.SignUp(signUpDto.Email, signUpDto.Password);
            return Ok();
        }
        catch (AmazonServiceException ex)
        {
            _logger.LogError("SignUp: {Message}", ex.Message);

            return StatusCode(
                (int)ex.StatusCode,
                new { StatusCode = ex.StatusCode, Message = ex.Message }
            );
        }
    }

    [HttpPost]
    [Route("confirm-sign-up")]
    public async Task<IActionResult> ConfirmSignUp([FromBody] ConfirmSignUpDto confirmSignUpDto)
    {
        try
        {
            await _cognitoRepository.ConfirmSignUp(confirmSignUpDto.Email, confirmSignUpDto.Code);
            return Ok();
        }
        catch (AmazonServiceException ex)
        {
            _logger.LogError("ConfirmSignUp: {Message}", ex.Message);

            return StatusCode(
                (int)ex.StatusCode,
                new { StatusCode = ex.StatusCode, Message = ex.Message }
            );
        }
    }

    [HttpPost]
    [Route("first-access")]
    public async Task<IActionResult> FirstAccess([FromBody] FirstAccessDto firstAccessDto)
    {
        try
        {
            var response = await _cognitoRepository.FirstAccess(
                firstAccessDto.Email,
                firstAccessDto.Name,
                firstAccessDto.Code,
                firstAccessDto.Password
            );
            return Ok(response);
        }
        catch (AmazonServiceException ex)
        {
            _logger.LogError("FirstAccess: {Message}", ex.Message);

            return StatusCode(
                (int)ex.StatusCode,
                new { StatusCode = ex.StatusCode, Message = ex.Message }
            );
        }
    }
}
