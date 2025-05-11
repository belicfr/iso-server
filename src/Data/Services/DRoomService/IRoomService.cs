using Iso.Data.Models.RoomModel;
using Iso.Data.Models.UserModel;

namespace Iso.Data.Services.DRoomService;

public interface IRoomService
{
    /// <summary>
    /// Loads the room's owner.
    /// </summary>
    /// <param name="roomId"></param>
    /// <returns></returns>
    Task<User?> GetOwnerAsync(string roomId);
    
    /// <summary>
    /// Loads the room's owner.
    /// </summary>
    /// <param name="room"></param>
    /// <returns></returns>
    Task<User?> GetOwnerAsync(Room room);
    
    
    /// <summary>
    /// Loads all the players banned in the room.
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyList<User>> GetBannedPlayersAsync(string roomId);
    
    /// <summary>
    /// Loads all the players banned in the room.
    /// </summary>
    /// <param name="room"></param>
    /// <returns></returns>
    Task<IReadOnlyList<User>> GetBannedPlayersAsync(Room room);
    
    
    /// <summary>
    /// Loads all the players having rights in the room.
    /// </summary>
    /// <param name="roomId"></param>
    /// <returns></returns>
    Task<IReadOnlyList<User>> GetPlayersWithRightsAsync(string roomId);
    
    /// <summary>
    /// Loads all the players having rights in the room.
    /// </summary>
    /// <param name="room"></param>
    /// <returns></returns>
    Task<IReadOnlyList<User>> GetPlayersWithRightsAsync(Room room);
}