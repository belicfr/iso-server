using Iso.Data.Models.HotelViewModel;

namespace Iso.Data.Services.Runtime.Promotions.Interfaces;

public interface IPromotionsRuntimeService
{
    /// <summary>
    /// Returns all promotions.
    /// (Last 5 promotions).
    /// </summary>
    /// <returns>
    /// Promotions hashset.
    /// </returns>
    Task<HashSet<Promotion>> GetAllPromotionsAsync();
}