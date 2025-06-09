using Iso.Data.Models.CreationModels;
using Iso.Data.Models.RoomModel;
using Iso.Data.Models.UserModel;
using Iso.Data.Services.DRoomService.Responses;
using Iso.Shared.Physic;

namespace Iso.Data.Services.DRoomService;

public interface IRoomService
{
    /// <summary>
    /// Creates a new room using the provided creation model.
    /// </summary>
    /// <param name="creationModel"></param>
    /// <param name="actorId"></param>
    /// <returns></returns>
    Task<Room?> CreateAsync(NewRoomCreationModel creationModel, string actorId);
    
    
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
    /// Saves a new name for the provided room.
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="actorId"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<NewNameResponse> SaveNewNameAsync(string roomId, string actorId, string name);


    /// <summary>
    /// Saves a new description for the provided room.
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="actorId"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    Task<NewDescriptionResponse> SaveNewDescriptionAsync(string roomId, string actorId, string description);


    /// <summary>
    /// Saves a new tag at given position for the provided room.
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="actorId"></param>
    /// <param name="position"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    Task<NewTagResponse> SaveNewTagAsync(string roomId, string actorId, int position, string tag);


    /// <summary>
    /// Attempts to enter a room.
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="actorId"></param>
    /// <returns></returns>
    Task<EnterResponse> AttemptEnterRoom(string roomId, string actorId);


    /// <summary>
    /// Returns to hotel view.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<GoToHotelViewResponse> GoToHotelView(string userId);


    /// <summary>
    /// Gives rights to the given user for the provided room. 
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="actorId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<AddRoomRightsResponse> AddRoomRightsAsync(string roomId, string actorId, string userId);


    /// <summary>
    /// Removes rights from given user for the provided room. 
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="actorId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<RemoveRoomRightsResponse> RemoveRoomRightsAsync(string roomId, string actorId, string userId);


    /// <summary>
    /// Bans the user from the provided room.
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="actorId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<BanUserFromRoomResponse> BanUserAsync(string roomId, string actorId, string userId);


    /// <summary>
    /// Unbans the given banned user from the provided room.
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="actorId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<UnbanUserFromRoomResponse> UnbanUserAsync(string roomId, string actorId, string userId);


    /// <summary>
    /// Unbans all banned users from the provided room.
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="actorId"></param>
    /// <returns></returns>
    Task<UnbanAllFromRoomResponse> UnbanAllAsync(string roomId, string actorId);
    
    
    /// <summary>
    /// Bans a new word in the provided room.
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="actorId"></param>
    /// <param name="word"></param>
    /// <returns></returns>
    Task<AddBannedWordResponse> AddBannedWordAsync(string roomId, string actorId, string word);
    
    
    /// <summary>
    /// Unbans a word from the provided room (if exists).
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="actorId"></param>
    /// <param name="word"></param>
    /// <returns></returns>
    Task<RemoveBannedWordResponse> RemoveBannedWordAsync(string roomId, string actorId, string word);
    
    
    /// <summary>
    /// Attempts to move given actor from and to given coords,
    /// in the provided room.
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="actorId"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    Task<List<Coord2D>?> MovePlayerAsync(string roomId, string actorId, Coord2D from, Coord2D to);
    
    
    /// <summary>
    /// Attempt to retrieve entry tile coordinate.
    /// </summary>
    /// <param name="roomId"></param>
    /// <returns></returns>
    Coord2D GetEntryCoord(Room room);
}