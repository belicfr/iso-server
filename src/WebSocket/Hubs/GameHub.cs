using System.Security.Claims;
using Iso.Data.Models.RoomModel;
using Iso.Data.Models.UserModel;
using Iso.Data.Services.DRoomService;
using Iso.Data.Services.DUserService;
using Iso.Shared.DTO;
using Iso.Shared.DTO.Public;
using Iso.Shared.DTO.Restricted;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using OneOf;

namespace Iso.WebSocket.Hubs;

public partial class GameHub(
    RoomService roomService,
    UserService userService,
    RoomRuntimeService roomRuntimeService,
    UserRuntimeService userRuntimeService): Hub
{
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        User? user = await GetUserBySso();

        if (user is not null)
        {
            string? currentRoomId = userRuntimeService.GetCurrentRoom(user.Id);

            if (currentRoomId is not null)
            {
                userRuntimeService.ClearCurrentRoom(user.Id);
                roomRuntimeService.RemovePlayer(currentRoomId, user.Id);
            }
        }
        
        await base.OnDisconnectedAsync(exception);
    }


    private List<PublicNavigatorRoomResponseModel> PrepareRooms(
        IEnumerable<Room> rooms)
    {
        return rooms
            .Select(PrepareRoom)
            .ToList();
    }
    
    private async Task<List<RestrictedRoomResponseModel>> PrepareRoomsForOwner(
        IEnumerable<Room> rooms)
    {
        List<RestrictedRoomResponseModel> restrictedRooms = new();

        foreach (Room room in rooms)
        {
            restrictedRooms.Add(await PrepareRoomForOwner(room));
        }
        
        return restrictedRooms;
    }

    private PublicNavigatorRoomResponseModel PrepareRoom(Room room)
    {
        PublicGroupResponseModel? group = null;
        
        if (room.Group is not null)
        {
            group = new PublicGroupResponseModel(
                room.Group.Id,
                room.Group.Name,
                room.Group.Description,
                room.Group.OwnerId,
                (int) room.Group.GroupMode,
                room.Group.RoomId,
                room.Group.CreatedAt);
        }
                
        return new PublicNavigatorRoomResponseModel(
            room.Id,
            room.Name,
            room.Description,
            roomRuntimeService.GetPlayers(room.Id).Count,
            room.PlayersLimit,
            room.Template,
            group,
            room.OwnerId,
            room.TagOne,
            room.TagTwo,
            room.IsPublic);
    }
    
    private async Task<RestrictedRoomResponseModel> PrepareRoomForOwner(Room room)
    {
        PublicGroupResponseModel? group = null;
        
        if (room.Group is not null)
        {
            group = new PublicGroupResponseModel(
                room.Group.Id,
                room.Group.Name,
                room.Group.Description,
                room.Group.OwnerId,
                (int) room.Group.GroupMode,
                room.Group.RoomId,
                room.Group.CreatedAt);
        }

        List<PublicAccountResponseModel> havingRights 
            = (await roomService.GetPlayersWithRightsAsync(room.Id))
                .Select(PrepareAccount)
                .ToList();

        return new RestrictedRoomResponseModel(
            room.Id,
            room.Name,
            room.Description,
            roomRuntimeService.GetPlayers(room.Id).Count,
            room.PlayersLimit,
            room.Template,
            group,
            room.OwnerId,
            room.TagOne,
            room.TagTwo,
            room.IsPublic,
            havingRights);
    }
    
    /// <summary>
    /// Retrieve SSO from Sub JWT claim.
    /// </summary>
    /// <returns></returns>
    private string? GetSso()
    {
        return Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    private async Task<User?> GetUserBySso()
    {
        string? sso = GetSso();
        
        if (string.IsNullOrWhiteSpace(sso))
        {
            return null;
        }
        
        return await userService.GetUserBySsoAsync(sso);
    }

    private PublicAccountResponseModel PrepareAccount(User user)
    {
        return new PublicAccountResponseModel(
            user.Id,
            user.UserName ?? "Unknown", 
            user.NormalizedUserName ?? "Unknown");
    }
}
