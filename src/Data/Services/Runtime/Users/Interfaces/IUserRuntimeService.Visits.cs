namespace Iso.Data.Services.Runtime.Users.Interfaces;

public partial interface IUserRuntimeService
{
    /// <summary>
    /// Sets the provided user's current room.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roomId"></param>
    public void SetCurrentRoom(string userId, string roomId);
    
    
    /// <summary>
    /// Clears the provided user's current room.
    /// </summary>
    /// <param name="userId"></param>
    public void ClearCurrentRoom(string userId);
    
    
    /// <summary>
    /// Gets the provided user's current room (if set).
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public string? GetCurrentRoom(string userId);

    
    /// <summary>
    /// Returns if the provided user currently is in a room.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public bool IsInRoom(string userId);
}