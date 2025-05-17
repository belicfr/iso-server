using Iso.Data.Models.RoomModel;

namespace Iso.Data.Services.Runtime.Rooms.Interfaces;

public partial interface IRoomRuntimeService
{
    /// <summary>
    /// Updates the concerned room's name.
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="name"></param>
    /// <returns>
    /// If the update is successful.
    /// </returns>
    public Task<bool> UpdateRoomNameAsync(string roomId, string name);
    
    
    /// <summary>
    /// Updates the concerned room's description.
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="description"></param>
    /// <returns>
    /// If the update is successful.
    /// </returns>
    public Task<bool> UpdateRoomDescriptionAsync(string roomId, string description);
    
    
    /// <summary>
    /// Updates the concerned room's first tag.
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="tagOne"></param>
    /// <returns>
    /// If the update is successful.
    /// </returns>
    public Task<bool> UpdateRoomTagOneAsync(string roomId, string tagOne);
    
    
    /// <summary>
    /// Updates the concerned room's second tag.
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="tagTwo"></param>
    /// <returns>
    /// If the update is successful.
    /// </returns>
    public Task<bool> UpdateRoomTagTwoAsync(string roomId, string tagTwo);
}