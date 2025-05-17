using Iso.Data.Models.RoomModel;
using Iso.Data.Models.UserModel;

namespace Iso.Data.Services.Runtime.Rooms.Interfaces;

public partial interface IRoomRuntimeService
{
    /// <summary>
    /// Try to return the room searching by its ID.
    /// </summary>
    /// <param name="roomId"></param>
    /// <returns>
    /// Room object if exists;
    /// NULL else.
    /// </returns>
    public Task<Room?> GetRoomByIdAsync(string roomId);
    
    
    /// <summary>
    /// Returns all public rooms.
    /// (First 100 rooms).
    /// </summary>
    /// <returns>
    /// Public rooms hashset.
    /// </returns>
    public Task<HashSet<Room>> GetAllPublicRoomsAsync();
    
    
    /// <summary>
    /// Returns all rooms.
    /// (First 100 rooms)
    /// </summary>
    /// <returns>
    /// Rooms hashset.
    /// </returns>
    public Task<HashSet<Room>> GetAllRoomsAsync();

    
    /// <summary>
    /// Returns all player's rooms.
    /// </summary>
    /// <param name="user"></param>
    /// <returns>
    /// Player's rooms hashset.
    /// </returns>
    public Task<HashSet<Room>> GetAllPlayerRoomsAsync(User user);
    
    /// <summary>
    /// Returns all player's rooms.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>
    /// Player's rooms hashset.
    /// </returns>
    public Task<HashSet<Room>> GetAllPlayerRoomsAsync(string userId);


    /// <summary>
    /// Attempts to create a new room.
    /// </summary>
    /// <param name="room"></param>
    /// <returns></returns>
    public Task CreateRoomAsync(Room room);
}