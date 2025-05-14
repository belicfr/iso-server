using Iso.Data.Models.RoomModel;
using Iso.Data.Models.UserModel;
using Iso.Shared.DTO.Restricted;
using Microsoft.AspNetCore.SignalR;

namespace Iso.WebSocket.Hubs;

public partial class GameHub
{
    public async Task SendUserInfo()
    {
        const string responseChannel = "ReceiveUserInfo";

        User? user = await GetUserBySso();

        if (user is null)
        {
            await Clients.Caller.SendAsync(responseChannel, null);
            return;
        }
        
        List<string> friendsIds = user.Friends?
            .Select(f => f.Id)
            .ToList() ?? new();
        
        await Clients.Caller.SendAsync(
            responseChannel, 
            new RestrictedAccountResponseModel(
                user.Id,
                user.UserName ?? "Unknown",
                user.NormalizedUserName ?? "Unknown",
                user.HomeRoomId,
                user.Crowns,
                friendsIds));
    }

    public async Task SendRoomEnterAttempt(string roomId)
    {
        const string responseChannel = "ReceiveRoomEnterAttempt";

        User? user = await GetUserBySso();
        Room? room = await roomService.GetRoomAsync(roomId);

        if (user is null)
        {
            await Clients.Caller.SendAsync(
                responseChannel,
                null);
            
            return;
        }

        if (room is null)
        {
            await Clients.Caller.SendAsync(
                responseChannel,
                null);
            
            return;
        }

        await Clients.Caller.SendAsync(
            responseChannel,
            roomService.AttemptEnterRoom(room, user));
    }

    public async Task SendGoToHotelView()
    {
        const string responseChannel = "ReceiveGoToHotelView";
        
        User? user = await GetUserBySso();

        if (user is null)
        {
            await Clients.Caller.SendAsync(responseChannel, null);

            return;
        }

        await Clients.Caller.SendAsync(
            responseChannel,
            roomService.GoToHotelView(user));
    }
}