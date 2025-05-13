using Iso.Data.Models.RoomModel;
using Iso.Data.Models.UserModel;

namespace Iso.Data.Services.DRoomService;

public interface IRoomService
{
    /// <summary>
    /// Loads room by its ID if exists.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Room?> GetRoomAsync(string id);
    
    /// <summary>
    /// Loads all rooms.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Room>> GetAllRoomsAsync();
    
    /// <summary>
    /// Loads all public rooms.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Room>> GetPublicRoomsAsync();
    
    /// <summary>
    /// Loads all player's rooms.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<IEnumerable<Room>> GetPlayerRoomsAsync(string userId);
    
    /// <summary>
    /// Loads all player's rooms.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<IEnumerable<Room>> GetPlayerRoomsAsync(User user);
    
    
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
    
    
    /// <summary>
    /// Attempts to enter a room.
    /// </summary>
    /// <param name="room"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    ServiceResponse AttemptEnterRoom(Room room, User user);

    /// <summary>
    /// Returns to hotel view.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    ServiceResponse GoToHotelView(User user);
}