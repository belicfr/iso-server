using Iso.Data.DbContexts;
using Iso.Data.Models.HotelViewModel;
using Iso.Data.Services.Runtime.Promotions.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Iso.Data.Services.Runtime.Promotions;

public class PromotionsRuntimeService(
    IDbContextFactory<GameDbContext> gameDbContext): IPromotionsRuntimeService
{
    private readonly HashSet<Promotion> _promotions = new();
    
    public async Task<HashSet<Promotion>> GetAllPromotionsAsync()
    {
        GameDbContext context = await gameDbContext.CreateDbContextAsync();
        
        HashSet<Promotion> promotions = context.Promotions
            .Take(5)
            .OrderBy(p => p.Position)
            .ToHashSet();

        foreach (Promotion promotion in promotions)
        {
            _promotions.Add(promotion);
        }
        
        return promotions;
    }
}