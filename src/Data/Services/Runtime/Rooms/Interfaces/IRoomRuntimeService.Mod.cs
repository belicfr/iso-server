using Iso.Data.Models.RoomModel;

namespace Iso.Data.Services.Runtime.Rooms.Interfaces;

public partial interface IRoomRuntimeService
{
    /// <summary>
    /// Bans user from provided room.
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="ban"></param>
    /// <returns>
    /// If ban is successful.
    /// </returns>
    public Task<bool> BanUserAsync(string roomId, RoomBan ban);


    /// <summary>
    /// Unbans user from provided room.
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="ban"></param>
    /// <returns>
    /// If unban is successful.
    /// </returns>
    public Task<bool> UnbanUserAsync(string roomId, RoomBan ban);
    
    
    /// <summary>
    /// Unbans all banned users from provided room.
    /// </summary>
    /// <param name="roomId"></param>
    /// <returns>
    /// If unbans are successful.
    /// </returns>
    public Task<bool> UnbanAllAsync(string roomId);


    /// <summary>
    /// Bans word from the provided room.
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="bannedWord"></param>
    /// <returns>
    /// If ban is successful.
    /// </returns>
    public Task<bool> BanWord(string roomId, RoomBannedWord bannedWord);
    
    
    /// <summary>
    /// Unbans word from the provided room.
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="bannedWord"></param>
    /// <returns>
    /// If unban is successful.
    /// </returns>
    public Task<bool> UnbanWord(string roomId, RoomBannedWord bannedWord);
}