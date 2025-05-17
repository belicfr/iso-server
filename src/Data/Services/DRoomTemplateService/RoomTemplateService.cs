using System.Collections;
using Iso.Data.DbContexts;
using Iso.Data.Models.RoomModel;
using Iso.Data.Services.Runtime.RoomTemplates;
using Microsoft.EntityFrameworkCore;

namespace Iso.Data.Services.DRoomTemplateService;

public class RoomTemplateService(
    GameDbContext gameDbContext,
    RoomTemplateRuntimeService roomTemplateRuntimeService): IRoomTemplateService
{
    public async Task<RoomTemplate?> GetRoomTemplateAsync(string roomTemplateId)
    {
        return await roomTemplateRuntimeService
            .GetRoomTemplateByIdAsync(roomTemplateId);
    }

    public async Task<IEnumerable<RoomTemplate>> GetAllRoomTemplatesAsync()
    {
        return await roomTemplateRuntimeService
            .GetRoomTemplatesAsync();
    }
}