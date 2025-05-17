using System.Security.Claims;
using Iso.Data.Models.RoomModel;
using Iso.Data.Models.UserModel;
using Iso.Data.Services.DRoomService;
using Iso.Data.Services.DRoomTemplateService;
using Iso.Data.Services.DUserService;
using Iso.Data.Services.Runtime.Rooms;
using Iso.Data.Services.Runtime.Rooms.Interfaces;
using Iso.Data.Services.Runtime.Users;
using Iso.Shared.DTO.Public;
using Iso.Shared.DTO.Restricted;
using Microsoft.AspNetCore.SignalR;

namespace Iso.WebSocket.Hubs;

public partial class GameHub(
    RoomService roomService,
    UserService userService,
    RoomTemplateService roomTemplateService,
    IRoomRuntimeService roomRuntimeService,
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
                roomRuntimeService.RemovePlayer(currentRoomId, user);
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

        
        HashSet<RoomRight> roomRights = room.RoomRights
            .ToHashSet();

        List<PublicAccountResponseModel> havingRights = new();

        foreach (RoomRight right in roomRights)
        {
            User? user = await userService.GetUserAsync(right.UserId);

            if (user is not null)
            {
                havingRights.Add(PrepareAccount(user));                
            }
        }


        HashSet<RoomBan> roomBans = room.RoomBans
            .ToHashSet();
        
        List<PublicAccountResponseModel> bannedPlayers = new();

        foreach (RoomBan ban in roomBans)
        {
            User? user = await userService.GetUserAsync(ban.UserId);

            if (user is not null)
            {
                bannedPlayers.Add(PrepareAccount(user));
            }
        }
        
        HashSet<RoomBannedWord> roomBannedWords = room.RoomBannedWords
            .ToHashSet();

        List<string> bannedWords = roomBannedWords
            .Select(e => e.BannedWord)
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
            havingRights,
            bannedPlayers,
            bannedWords);
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
