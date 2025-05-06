using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iso.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController: ControllerBase
{
    [HttpGet]
    [Authorize]
    public IActionResult Get()
    {
        return Ok("Hello World!");
    }
}