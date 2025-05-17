using Iso.Data.Models.RoomModel;
using Iso.Data.Models.UserModel;

namespace Iso.Data.Services.Runtime.Rooms.Interfaces;

public partial interface IRoomRuntimeService
{
    /// <summary>
    /// Gives room rights to provided user.
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="right"></param>
    /// <returns>
    /// If rights giving is successful.
    /// </returns>
    public Task<bool> GiveRoomRightsAsync(string roomId, RoomRight right);
    
    
    /// <summary>
    /// Removes room rights for provided user.
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="right"></param>
    /// <returns>
    /// If rights removing is successful.
    /// </returns>
    public Task<bool> RemoveRoomRightsAsync(string roomId, RoomRight right);
}