using Iso.Data.Models.RoomModel;

namespace Iso.Data.Services.Runtime.RoomTemplates.Interfaces;

public interface IRoomTemplateRuntimeService
{
    /// <summary>
    /// Returns room template searching by its ID (if exists).
    /// </summary>
    /// <param name="roomTemplateId"></param>
    /// <returns>
    /// Room template object if exists;
    /// else NULL.
    /// </returns>
    public Task<RoomTemplate?> GetRoomTemplateByIdAsync(string roomTemplateId);
    
    
    /// <summary>
    /// Returns all room templates.
    /// </summary>
    /// <returns></returns>
    public Task<HashSet<RoomTemplate>> GetRoomTemplatesAsync();
}