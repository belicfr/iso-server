using Iso.Data.Models.RoomModel;
using Microsoft.AspNetCore.Mvc;
using Iso.Data.Services.DRoomTemplateService;
using Microsoft.AspNetCore.Authorization; // Assuming this is where your service is located

namespace Iso.API.Controllers;

[ApiController]
[Authorize]
[Route("/api/[controller]/[action]")]
public class RoomTemplateController(
    RoomTemplateService roomTemplateService) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<RoomTemplate>> GetRoomTemplates()
    {
        return Ok(roomTemplateService.GetAllRoomTemplatesAsync());
    }
}
