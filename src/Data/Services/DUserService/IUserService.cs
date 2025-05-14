using Iso.Data.Models.RoomModel;
using Iso.Data.Models.UserModel;

namespace Iso.Data.Services.DUserService;

public interface IUserService
{
    /// <summary>
    /// Loads the user's account.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<User?> GetUserAsync(string userId);
    
    /// <summary>
    /// Loads the user's account using its SSO token.
    /// </summary>
    /// <param name="sso"></param>
    /// <returns></returns>
    Task<User?> GetUserBySsoAsync(string sso);
    
    /// <summary>
    /// Loads the user's home room (if exists).
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<Room?> GetHomeRoomAsync(string userId);
    
    /// <summary>
    /// Loads the user's rooms.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<IReadOnlyList<Room>> GetRoomsForUserAsync(string userId);
    
    /// <summary>
    /// Returns if user id exists.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<bool> IsUserExistingAsync(string userId);
}