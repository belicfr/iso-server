using Iso.Data.Models.RoomModel;
using Iso.Data.Models.UserModel;

namespace Iso.Data.Services.Runtime.Users.Interfaces;

public partial interface IUserRuntimeService
{
    /// <summary>
    /// Try to return the user searching by its ID.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>
    /// User object if exists;
    /// NULL else.
    /// </returns>
    public Task<User?> GetUserByIdAsync(string userId);
    
    
    /// <summary>
    /// Try to return the user searching by its SSO token.
    /// </summary>
    /// <param name="sso"></param>
    /// <returns>
    /// User object if exists;
    /// NULL else.
    /// </returns>
    public Task<User?> GetUserBySsoAsync(string sso);
}