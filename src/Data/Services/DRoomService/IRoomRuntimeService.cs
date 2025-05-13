using Iso.Data.Models.UserModel;

namespace Iso.Data.Services.DRoomService;

public interface IRoomRuntimeService
{
    /// <summary>
    /// Retrieves players in the provided room.
    /// </summary>
    /// <param name="roomId"></param>
    /// <returns></returns>
    public List<string> GetPlayers(string roomId);
    
    /// <summary>
    /// Retrieves players count in the provided room.
    /// </summary>
    /// <param name="roomId"></param>
    /// <returns></returns>
    public int GetPlayersCount(string roomId);
    
    /// <summary>
    /// Adds a player to the players in room dictionnary.
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="userId"></param>
    public void AddPlayer(string roomId, string userId);
    
    /// <summary>
    /// Removes a player from the players in room dictionnary.
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="userId"></param>
    public void RemovePlayer(string roomId, string userId);

    /// <summary>
    /// Gets if provided user currently is in the provided room.
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public bool IsPlayerInRoom(string roomId, string userId);
}