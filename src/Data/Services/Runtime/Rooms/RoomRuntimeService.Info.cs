using Iso.Data.Models.RoomModel;

namespace Iso.Data.Services.Runtime.Rooms;

public partial class RoomRuntimeService
{
    public async Task<bool> UpdateRoomNameAsync(string roomId, string name)
    {
        Room? room = await GetRoomByIdAsync(roomId);
        
        if (room is null) return false;
        
        room.Name = name;

        return true;
    }

    public async Task<bool> UpdateRoomDescriptionAsync(string roomId, string description)
    {
        Room? room = await GetRoomByIdAsync(roomId);
        
        if (room is null) return false;
        
        room.Description = description;
        
        return true;
    }

    public async Task<bool> UpdateRoomTagOneAsync(string roomId, string tagOne)
    {
        Room? room = await GetRoomByIdAsync(roomId);
        
        if (room is null) return false;
        
        room.TagOne = tagOne;
        
        return true;
    }

    public async Task<bool> UpdateRoomTagTwoAsync(string roomId, string tagTwo)
    {
        Room? room = await GetRoomByIdAsync(roomId);
        
        if (room is null) return false;
        
        room.TagTwo = tagTwo;
        
        return true;
    }
}