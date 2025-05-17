using Iso.Data.Models.RoomModel;
using Iso.Data.Models.UserModel;

namespace Iso.Data.Services.Runtime.Rooms;

public partial class RoomRuntimeService
{
    public async Task<bool> GiveRoomRightsAsync(string roomId, RoomRight right)
    {
        Room? room = await GetRoomByIdAsync(roomId);

        if (room is null)
        {
            return false;
        }
        
        room.RoomRights
            .Add(right);

        return true;
    }
    

    public async Task<bool> RemoveRoomRightsAsync(string roomId, RoomRight right)
    {
        Room? room = await GetRoomByIdAsync(roomId);

        if (room is null)
        {
            return false;
        }
        
        room.RoomRights
            .Remove(right);
        
        return true;
    }
}