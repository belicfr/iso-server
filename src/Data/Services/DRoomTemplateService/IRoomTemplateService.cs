using System.Collections;
using Iso.Data.Models.RoomModel;

namespace Iso.Data.Services.DRoomTemplateService;

public interface IRoomTemplateService
{
    Task<RoomTemplate?> GetRoomTemplateAsync(string roomTemplateId);
    
    
    /// <summary>
    /// Retrieve all room templates.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<RoomTemplate>> GetAllRoomTemplatesAsync();
}