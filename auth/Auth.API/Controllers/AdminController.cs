using Amazon.CognitoIdentityProvider.Model;
using Amazon.Runtime;
using Auth.API.Dtos;
using Auth.API.Models;
using Auth.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AdminController : ControllerBase
{
    private readonly ICognitoRepository<ClientModel> _cognitoRepository;
    private readonly ILogger<AdminController> _logger;

    public AdminController(
        ICognitoRepository<ClientModel> cognitoRepository,
        ILogger<AdminController> logger
    )
    {
        _cognitoRepository = cognitoRepository;
        _logger = logger;
    }

    private bool IsCompanyEmail(string email)
    {
        return email.EndsWith("@rafasrent.com");
    }


    [HttpPost]
    [AllowAnonymous]
    [Route("sign-in")]
    public async Task<IActionResult> SignIn([FromBody] SignInDto signInDto)
    {
        try
        {
            if (!IsCompanyEmail(signInDto.Email))
            {
                return BadRequest(new { Message = "Unauthorized: Only company emails are allowed." });
            }

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
    [AllowAnonymous]
    [Route("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
    {
        try
        {
            if (!IsCompanyEmail(forgotPasswordDto.Email))
            {
                return BadRequest(new { Message = "Unauthorized: Only company emails are allowed." });
            }

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
    [AllowAnonymous]
    [Route("confirm-forgot-password")]
    public async Task<IActionResult> ConfirmForgotPassword(
        [FromBody] ConfirmForgotPasswordDto confirmForgotPasswordDto
    )
    {
        try
        {
            if (!IsCompanyEmail(confirmForgotPasswordDto.Email))
            {
                return BadRequest(new { Message = "Unauthorized: Only company emails are allowed." });
            }

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
    [AllowAnonymous]
    [Route("sign-up")]
    public async Task<IActionResult> SignUp([FromBody] SignUpDto signUpDto)
    {
        try
        {
            if (!IsCompanyEmail(signUpDto.Email))
            {
                return BadRequest(new { Message = "Unauthorized: Only company emails are allowed." });
            }


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
    [AllowAnonymous]
    [Route("confirm-sign-up")]
    public async Task<IActionResult> ConfirmSignUp([FromBody] ConfirmSignUpDto confirmSignUpDto)
    {
        try
        {
            if (!IsCompanyEmail(confirmSignUpDto.Email))
            {
                return BadRequest(new { Message = "Unauthorized: Only company emails are allowed." });
            }

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
    [Authorize]
    [Route("first-access")]
    public async Task<IActionResult> FirstAccess([FromBody] FirstAccessDto firstAccessDto)
    {
        try
        {
            if (!IsCompanyEmail(firstAccessDto.Email))
            {
                return BadRequest(new { Message = "Unauthorized: Only company emails are allowed." });
            }

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

    [HttpPost]
    [Authorize]
    [Route("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailDto verifyEmailDto)
    {
        try
        {
            if (!IsCompanyEmail(verifyEmailDto.Email))
            {
                return BadRequest(new { Message = "Unauthorized: Only company emails are allowed." });
            }

            await _cognitoRepository.VerifyEmail(verifyEmailDto.Email, verifyEmailDto.Code);
            return Ok();
        }
        catch (AmazonServiceException ex)
        {
            _logger.LogError("VerifyEmail: {Message}", ex.Message);

            return StatusCode(
                (int)ex.StatusCode,
                new { StatusCode = ex.StatusCode, Message = ex.Message }
            );
        }
    }

    [HttpPost]
    [Authorize]
    [Route("update-user")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto updateUserDto)
    {
        try
        {
            if (!IsCompanyEmail(updateUserDto.Email))
            {
                return BadRequest(new { Message = "Unauthorized: Only company emails are allowed." });
            }

            await _cognitoRepository.UpdateUser(updateUserDto.Email, updateUserDto.Name);
            return Ok();
        }
        catch (AmazonServiceException ex)
        {
            _logger.LogError("UpdateUser: {Message}", ex.Message);

            return StatusCode(
                (int)ex.StatusCode,
                new { StatusCode = ex.StatusCode, Message = ex.Message }
            );
        }
    }

    [HttpPost]
    [Authorize]
    [Route("delete-user")]
    public async Task<IActionResult> DeleteUser([FromBody] DeleteUserDto deleteUserDto)
    {
        try
        {
            if (!IsCompanyEmail(deleteUserDto.Email))
            {
                return BadRequest(new { Message = "Unauthorized: Only company emails are allowed." });
            }

            await _cognitoRepository.DeleteUser(deleteUserDto.Email);
            return Ok();
        }
        catch (AmazonServiceException ex)
        {
            _logger.LogError("DeleteUser: {Message}", ex.Message);

            return StatusCode(
                (int)ex.StatusCode,
                new { StatusCode = ex.StatusCode, Message = ex.Message }
            );
        }
    }

    [HttpPost]
    [Authorize]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
    {
        try
        {
            var response = await _cognitoRepository.RefreshToken(refreshTokenDto.RefreshToken);
            return Ok(response);
        }
        catch (AmazonServiceException ex)
        {
            _logger.LogError("RefreshToken: {Message}", ex.Message);

            return StatusCode(
                (int)ex.StatusCode,
                new { StatusCode = ex.StatusCode, Message = ex.Message }
            );
        }
    }
}
