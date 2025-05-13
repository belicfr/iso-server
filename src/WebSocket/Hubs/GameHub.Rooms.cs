using Iso.Data.Models.RoomModel;
using Iso.Data.Models.UserModel;
using Iso.Shared.DTO.Public;
using Microsoft.AspNetCore.SignalR;

namespace Iso.WebSocket.Hubs;

public partial class GameHub
{
    public async Task SendAllRooms()
    {
        const string responseChannel = "ReceiveAllRooms";

        IEnumerable<PublicNavigatorRoomResponseModel> rooms
            = PrepareRooms(await roomService.GetAllRoomsAsync());
        
        await Clients.Caller.SendAsync(responseChannel, rooms);
    }

    public async Task SendPublicRooms()
    {
        const string responseChannel = "ReceivePublicRooms";
        
        IEnumerable<PublicNavigatorRoomResponseModel> rooms
            = PrepareRooms(await roomService.GetPublicRoomsAsync());
        
        await Clients.Caller.SendAsync(responseChannel, rooms);
    }

    public async Task SendPlayerRooms()
    {
        const string responseChannel = "ReceivePlayerRooms";
        
        User? user = await GetUserBySso();

        if (user is null)
        {
            await Clients.Caller.SendAsync(
                responseChannel, 
                Array.Empty<PublicNavigatorRoomResponseModel>());

            return;
        }

        IEnumerable<PublicNavigatorRoomResponseModel> rooms
            = PrepareRooms(await userService.GetRoomsForUserAsync(
                user.Id));
        
        await Clients.Caller.SendAsync(responseChannel, rooms);
    }

    public async Task SendNewRoomName(string roomId, string name)
    {
        const string responseChannel = "ReceiveNewRoomName";
        
        Room? room = await roomService.GetRoomAsync(roomId);

        if (room is null)
        {
            await Clients.Caller.SendAsync(
                responseChannel,
                null);

            return;
        }

        await Clients.Caller.SendAsync(
            responseChannel,
            await roomService.SaveNewNameAsync(roomId, name));
    }
    
    public async Task SendNewRoomDescription(string roomId, string description)
    {
        const string responseChannel = "ReceiveNewRoomDescription";
        
        Room? room = await roomService.GetRoomAsync(roomId);

        if (room is null)
        {
            await Clients.Caller.SendAsync(
                responseChannel,
                null);

            return;
        }

        await Clients.Caller.SendAsync(
            responseChannel,
            await roomService.SaveNewDescriptionAsync(roomId, description));
    }
    
    public async Task SendNewRoomTag(string roomId, int position, string tag)
    {
        const string responseChannel = "ReceiveNewRoomDescription";
        
        Room? room = await roomService.GetRoomAsync(roomId);

        if (room is null)
        {
            await Clients.Caller.SendAsync(
                responseChannel,
                null);

            return;
        }

        await Clients.Caller.SendAsync(
            responseChannel,
            await roomService.SaveNewTagAsync(roomId, position, tag));
    }
}