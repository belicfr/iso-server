using Iso.Data.Models.HotelViewModel;
using Iso.Data.Services.Runtime.Promotions;

namespace Iso.Data.Services.DPromotionService;

public class PromotionService(
    PromotionsRuntimeService promotionsRuntimeService): IPromotionService
{
    public async Task<IEnumerable<Promotion>> GetAllPromotionsAsync()
    {
        return await promotionsRuntimeService
            .GetAllPromotionsAsync();
    }
}