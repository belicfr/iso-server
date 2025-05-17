using Iso.Data.DbContexts;
using Iso.Data.Models.RoomModel;
using Iso.Data.Services.Runtime.RoomTemplates.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Iso.Data.Services.Runtime.RoomTemplates;

public class RoomTemplateRuntimeService(
    IDbContextFactory<GameDbContext> gameDbContext): IRoomTemplateRuntimeService
{
    private readonly HashSet<RoomTemplate> _templates = new();

    public async Task<RoomTemplate?> GetRoomTemplateByIdAsync(string roomTemplateId)
    {
        RoomTemplate? roomTemplate = _templates
            .FirstOrDefault(x => x.Id == roomTemplateId);

        if (roomTemplate is not null)
        {
            return roomTemplate;
        }

        GameDbContext context = await gameDbContext.CreateDbContextAsync();
        
        roomTemplate = await context.RoomTemplates
            .FirstOrDefaultAsync(x => x.Id == roomTemplateId);

        if (roomTemplate is not null)
        {
            _templates.Add(roomTemplate);
        }
        
        return roomTemplate;
    }

    public async Task<HashSet<RoomTemplate>> GetRoomTemplatesAsync()
    {
        GameDbContext dbContext = await gameDbContext.CreateDbContextAsync();
        
        HashSet<RoomTemplate> roomTemplates = dbContext.RoomTemplates
            .AsNoTracking()
            .Take(50)
            .ToHashSet();

        foreach (RoomTemplate template in roomTemplates)
        {
            Console.WriteLine($"tilescount= {template.TilesCount}");
            _templates.Add(template);
        }

        return roomTemplates;
    }
}