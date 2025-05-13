using System.Collections;
using Iso.Data.Models.RoomModel;

namespace Iso.Data.Services.DRoomTemplateService;

public interface IRoomTemplateService
{
    /// <summary>
    /// Retrieve all room templates.
    /// </summary>
    /// <returns></returns>
    IEnumerable<RoomTemplate> GetAllRoomTemplatesAsync();
}