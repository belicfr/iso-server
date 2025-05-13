using System.Collections;
using Iso.Data.DbContexts;
using Iso.Data.Models.RoomModel;
using Microsoft.EntityFrameworkCore;

namespace Iso.Data.Services.DRoomTemplateService;

public class RoomTemplateService(
    GameDbContext gameDbContext): IRoomTemplateService
{
    public IEnumerable<RoomTemplate> GetAllRoomTemplatesAsync()
    {
        return gameDbContext.RoomTemplates
            .AsNoTracking()
            .ToList();
    }
}