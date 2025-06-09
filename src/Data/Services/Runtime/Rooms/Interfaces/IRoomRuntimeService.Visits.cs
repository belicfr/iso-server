using Iso.Data.Models.UserModel;

namespace Iso.Data.Services.Runtime.Rooms.Interfaces;

public partial interface IRoomRuntimeService
{
    /// <summary>
    /// Retrieves players in the provided room.
    /// </summary>
    /// <param name="roomId"></param>
    /// <returns></returns>
    public HashSet<User> GetPlayers(string roomId);
    
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
    /// <param name="user"></param>
    public void AddPlayer(string roomId, User user);
    
    /// <summary>
    /// Removes a player from the players in room dictionnary.
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="user"></param>
    public void RemovePlayer(string roomId, User user);

    /// <summary>
    /// Gets if provided user currently is in the provided room.
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    public bool IsPlayerInRoom(string roomId, User user);
}