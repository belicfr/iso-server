using Iso.Data.Models.EventDispatchers.Rooms;
using Iso.Data.Models.RoomModel;
using Iso.Data.Services.Runtime.Rooms.Interfaces;
using Iso.Shared.DTO.Public;
using Iso.WebSocket.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Iso.WebSocket.EventDispatchers.Rooms;

public class RoomEventDispatcher(
    IHubContext<GameHub> hubContext,
    IRoomRuntimeService roomRuntimeService): IRoomEventDispatcher
{
    public async Task NotifyRoomUpdatedAsync(Room room)
    {
        const string responseChannel = "ReceiveRoomUpdate";
        
        IReadOnlyList<string> playersInRoomSsos = roomRuntimeService
            .GetPlayers(room.Id)
            .Select(p => p.Sso)
            .ToList();
        
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

        PublicNavigatorRoomResponseModel roomResponseModel = new(
            room.Id,
            room.Name,
            room.Description,
            playersInRoomSsos.Count,
            room.PlayersLimit,
            room.Template,
            groupResponseModel,
            room.OwnerId,
            room.TagOne,
            room.TagTwo,
            room.IsPublic,
            room.Thumbnail);

        await hubContext.Clients.Users(playersInRoomSsos)
            .SendAsync(
                responseChannel, 
                roomResponseModel);
    }
}