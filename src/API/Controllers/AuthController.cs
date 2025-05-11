using CommonUtilities;
using Iso.API.DTO;
using Iso.Data.Managers;
using Iso.Data.Models.UserModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using RegisterRequest = Iso.Data.Models.Requests.RegisterRequest;

namespace Iso.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController(
    IConfiguration configuration,
    CompleteUserManager userManager): ControllerBase
{
    private readonly SsoUtilities _ssoUtilities = new (configuration);
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest? registerRequest)
    {
        if (registerRequest == null)
        {
            return BadRequest("Registration data is required.");
        }

        if (string.IsNullOrWhiteSpace(registerRequest.UserName)
            || string.IsNullOrWhiteSpace(registerRequest.Email)
            || string.IsNullOrWhiteSpace(registerRequest.Password))
        {
            return BadRequest("All fields (username, email, password) are required.");
        }
        
        User user = new User
        {
            UserName = registerRequest.UserName,
            Email = registerRequest.Email
        };
        
        IdentityResult result = await userManager.CreateAsync(user, registerRequest.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.Select(e => e.Description));
        }
        
        return Ok();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest? loginRequest)
    {
        if (loginRequest == null)
        {
            return BadRequest("Login data is required.");
        }

        if (string.IsNullOrWhiteSpace(loginRequest.Email)
            || string.IsNullOrWhiteSpace(loginRequest.Password))
        {
            return BadRequest("All fields (email, password) are required.");
        }
        
        User? user = await userManager.FindByEmailAsync(loginRequest.Email);
        
        const string unauthorizedErrorMessage = "The email or password is incorrect.";

        if (user == null)
        {
            return Unauthorized(unauthorizedErrorMessage);
        }
        
        bool isValidPassword
            = await userManager.CheckPasswordAsync(user, loginRequest.Password);

        if (!isValidPassword)
        {
            return Unauthorized(unauthorizedErrorMessage);
        }

        string jwt = _ssoUtilities.GenerateJwtToken(user.Sso);
        
        return Ok(new AuthResponseModel(jwt));
    }
}