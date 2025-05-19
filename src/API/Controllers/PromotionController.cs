using Iso.Data.Services.DPromotionService;
using Iso.Shared.DTO.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iso.API.Controllers;

[ApiController]
[Route("/api/[controller]/[action]")]
public class PromotionController(
    PromotionService promotionService): ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> All()
    {
        HashSet<PublicPromotionResponseModel> promotionResponseModels 
            = (await promotionService.GetAllPromotionsAsync())
                .Select(p => new PublicPromotionResponseModel(
                    p.Id,
                    p.Title,
                    p.ThumbnailPath,
                    p.Action,
                    p.Position))
                .ToHashSet();
        
        return Ok(promotionResponseModels);
    }
}