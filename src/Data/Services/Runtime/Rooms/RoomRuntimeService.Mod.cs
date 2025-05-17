using Iso.Data.Models.RoomModel;

namespace Iso.Data.Services.Runtime.Rooms;

public partial class RoomRuntimeService
{
    public async Task<bool> BanUserAsync(string roomId, RoomBan ban)
    {
        Room? room = await GetRoomByIdAsync(roomId);

        if (room is null)
        {
            return false;
        }
        
        room.RoomBans
            .Add(ban);

        return true;
    }

    
    public async Task<bool> UnbanUserAsync(string roomId, RoomBan ban)
    {
        Room? room = await GetRoomByIdAsync(roomId);

        if (room is null)
        {
            return false;
        }
        
        room.RoomBans
            .Remove(ban);

        return true;
    }


    public async Task<bool> UnbanAllAsync(string roomId)
    {
        Room? room = await GetRoomByIdAsync(roomId);

        if (room is null)
        {
            return false;
        }
        
        room.RoomBans
            .Clear();

        return true;
    }

    
    public async Task<bool> BanWord(string roomId, RoomBannedWord bannedWord)
    {
        Room? room = await GetRoomByIdAsync(roomId);

        if (room is null)
        {
            return false;
        }
        
        room.RoomBannedWords
            .Add(bannedWord);
        
        return true;
    }

    
    public async Task<bool> UnbanWord(string roomId, RoomBannedWord bannedWord)
    {
        Room? room = await GetRoomByIdAsync(roomId);

        if (room is null)
        {
            return false;
        }
        
        room.RoomBannedWords
            .Remove(bannedWord);
        
        return true;
    }
}