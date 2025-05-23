using Iso.Data.Models.RoomModel;
using Iso.Data.Models.UserModel;
using Iso.Data.Services.DRoomService.Responses;
using Iso.Shared.DTO.Public;
using Iso.Shared.DTO.Restricted;
using Iso.WebSocket.Response;
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

        string? actorId = (await GetUserBySso())?
            .Id;

        EnterResponse response = EnterResponse.FAIL;
        SenderResponseCode code = SenderResponseCode.FAIL;

        if (actorId is not null)
        {
            response = await roomService.AttemptEnterRoom(roomId, actorId);
            
            code = response == EnterResponse.SUCCESS
                ? SenderResponseCode.SUCCESS
                : SenderResponseCode.FAIL;
        }

        string? message;

        switch (response)
        {
            case EnterResponse.SUCCESS:
                message = null;
                break;
            
            case EnterResponse.ROOM_NOT_EXISTS:
                message = "Room does not exist";
                break;
            
            case EnterResponse.ROOM_FULL:
                message = "Room is full";
                break;
            
            case EnterResponse.USER_BANNED:
                message = "You have been banned from this room. You can not enter until the owner debans you.";
                break;
            
            default:
                message = "An error occured. (Server Error)";
                break;
        }

        List<object>? props = null;

        Room? room = await roomService.GetRoomAsync(roomId);

        if (room is not null && code == SenderResponseCode.SUCCESS)
        {
            PublicGroupResponseModel? groupResponseModel = null;

            if (room.Group is not null)
            {
                groupResponseModel = new PublicGroupResponseModel(
                    room.Group.Id,
                    room.Group.Name,
                    room.Group.Description,
                    room.Group.OwnerId,
                    (int) room.Group.GroupMode,
                    room.Group.RoomId,
                    room.Group.CreatedAt);
            }

            PublicNavigatorRoomResponseModel roomResponseModel = new PublicNavigatorRoomResponseModel(
                room.Id,
                room.Name,
                room.Description,
                roomRuntimeService.GetPlayersCount(roomId),
                room.PlayersLimit,
                room.Template,
                groupResponseModel,
                room.OwnerId,
                room.TagOne,
                room.TagTwo,
                room.IsPublic,
                room.Thumbnail);

            props = [roomResponseModel];
        }
        
        await Clients.Caller.SendAsync(
            responseChannel,
            new SenderResponse(
                code,
                message,
                props));
    }

    public async Task SendGoToHotelView()
    {
        const string responseChannel = "ReceiveGoToHotelView";
        
        User? user = await GetUserBySso();
        
        GoToHotelViewResponse response = GoToHotelViewResponse.FAIL;
        SenderResponseCode code = SenderResponseCode.FAIL;

        if (user is not null)
        {
            response = await roomService.GoToHotelView(user.Id);
            code = response == GoToHotelViewResponse.SUCCESS
                ? SenderResponseCode.SUCCESS
                : SenderResponseCode.FAIL;
        }
        
        string? message;

        switch (response)
        {
            case GoToHotelViewResponse.SUCCESS:
                message = null;
                break;
            
            default:
                message = "An error occured. (Server Error)";
                break;
        }

        await Clients.Caller.SendAsync(
            responseChannel,
            new SenderResponse(
                code,
                message));
    }
}