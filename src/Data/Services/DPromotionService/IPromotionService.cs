using Iso.Data.Models.HotelViewModel;

namespace Iso.Data.Services.DPromotionService;

public interface IPromotionService
{
    /// <summary>
    /// Loads all promotions.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Promotion>> GetAllPromotionsAsync();
}