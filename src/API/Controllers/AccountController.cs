using Iso.Data.Managers;
using Iso.Data.Models.UserModel;
using Iso.Shared.DTO.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iso.API.Controllers;

[ApiController]
[Route("/api/[controller]/[action]")]
public class AccountController(
    CompleteUserManager userManager): ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetPlayerById(
        [FromQuery] string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("User ID is required.");
        }
        
        User? user = await userManager.FindByIdAsync(id);

        if (user == null)
        {
            return BadRequest("User not found.");
        }
        
        return Ok(new PublicApiAccountResponseModel(
            user.Id, 
            user.UserName ?? "Unknown", 
            user.NormalizedUserName ?? "UNKNOWN"));
    }
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetPlayerByName(
        [FromQuery] string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            return BadRequest("Username is required.");
        }
        
        User? user = await userManager.FindByNameAsync(userName);

        if (user == null)
        {
            return BadRequest("User not found.");
        }
        
        return Ok(new PublicApiAccountResponseModel(
            user.Id, 
            user.UserName ?? "Unknown", 
            user.NormalizedUserName ?? "UNKNOWN"));
    }
}