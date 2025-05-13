using Iso.Data.DbContexts;
using Iso.Data.Models.RoomModel;
using Iso.Data.Models.UserModel;
using Iso.Data.Services.DUserService;
using Iso.Shared.DTO.Public;
using Microsoft.EntityFrameworkCore;

namespace Iso.Data.Services.DRoomService;

public class RoomService(
    AuthDbContext authDbContext,
    GameDbContext gameDbContext,
    RoomRuntimeService roomRuntimeService,
    UserRuntimeService userRuntimeService): IRoomService
{
    public async Task<Room?> GetRoomAsync(string id)
    {
        return await gameDbContext.Rooms
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Room>> GetAllRoomsAsync()
    {
        List<Room> rooms = await gameDbContext.Rooms
            .Include(r => r.RoomBans)
            .Include(r => r.RoomRights)
            .Include(r => r.RoomTags)
            .Include(r => r.RoomBannedWords)
            .AsNoTracking()
            .ToListAsync();
        
        return rooms;
    }

    public async Task<IEnumerable<Room>> GetPublicRoomsAsync()
    {
        List<Room> rooms = await gameDbContext.Rooms
            .Include(r => r.RoomBans)
            .Include(r => r.RoomRights)
            .Include(r => r.RoomTags)
            .Include(r => r.RoomBannedWords)
            .AsNoTracking()
            .Where(r => r.IsPublic)
            .ToListAsync();
        
        return rooms;
    }

    public async Task<IEnumerable<Room>> GetPlayerRoomsAsync(string userId)
    {
        List<Room> rooms = await gameDbContext.Rooms
            .Include(r => r.RoomBans)
            .Include(r => r.RoomRights)
            .Include(r => r.RoomTags)
            .Include(r => r.RoomBannedWords)
            .AsNoTracking()
            .Where(r => r.OwnerId == userId)
            .ToListAsync();
        
        return rooms;
    }

    public async Task<IEnumerable<Room>> GetPlayerRoomsAsync(User user)
    {
        return await GetPlayerRoomsAsync(user.Id);
    }


    public async Task<User?> GetOwnerAsync(string roomId)
    {
        Room? room = gameDbContext.Rooms
            .AsNoTracking()
            .FirstOrDefault(r => r.Id == roomId);

        if (room is null)
        {
            return null;
        }

        return await GetOwnerAsync(room);
    }

    public async Task<User?> GetOwnerAsync(Room room)
    {
        return await authDbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == room.OwnerId);
    }


    public async Task<IReadOnlyList<User>> GetBannedPlayersAsync(string roomId)
    {
        List<string> banIds = await gameDbContext.RoomBans
            .AsNoTracking()
            .Where(b => b.RoomId == roomId)
            .Select(b => b.UserId)
            .ToListAsync();

        if (!banIds.Any())
        {
            return Array.Empty<User>();
        }

        return await authDbContext.Users
            .AsNoTracking()
            .Where(u => banIds.Contains(u.Id))
            .ToListAsync();
    }

    public async Task<IReadOnlyList<User>> GetBannedPlayersAsync(Room room)
    {
        return await GetBannedPlayersAsync(room.Id);
    }


    public async Task<IReadOnlyList<User>> GetPlayersWithRightsAsync(string roomId)
    {
        List<string> rightIds = await gameDbContext.RoomRights
            .AsNoTracking()
            .Where(b => b.RoomId == roomId)
            .Select(b => b.UserId)
            .ToListAsync();

        if (!rightIds.Any())
        {
            return Array.Empty<User>();
        }

        return await authDbContext.Users
            .AsNoTracking()
            .Where(u => rightIds.Contains(u.Id))
            .ToListAsync();
    }

    public async Task<IReadOnlyList<User>> GetPlayersWithRightsAsync(Room room)
    {
        return await GetPlayersWithRightsAsync(room.Id);
    }


    public async Task<ServiceResponse> SaveNewNameAsync(string roomId, string name)
    {
        Room? room = await GetRoomAsync(roomId);

        if (room is null)
        {
            return new ServiceResponse(
                ServiceResponseCode.FAIL,
                "Room not found");
        }
        
        room.Name = name;
        await gameDbContext.SaveChangesAsync();

        return new ServiceResponse(ServiceResponseCode.SUCCESS);
    }

    public async Task<ServiceResponse> SaveNewNameAsync(Room room, string name)
    {
        return await SaveNewNameAsync(room.Id, name);
    }


    public async Task<ServiceResponse> SaveNewDescriptionAsync(string roomId, string description)
    {
        Room? room = await GetRoomAsync(roomId);

        if (room is null)
        {
            return new ServiceResponse(
                ServiceResponseCode.FAIL,
                "Room not found");
        }
        
        room.Description = description;
        await gameDbContext.SaveChangesAsync();
        
        return new ServiceResponse(ServiceResponseCode.SUCCESS);
    }

    public async Task<ServiceResponse> SaveNewDescriptionAsync(Room room, string description)
    {
        return await SaveNewDescriptionAsync(room.Id, description);
    }


    public async Task<ServiceResponse> SaveNewTagAsync(string roomId, int position, string tag)
    {
        if (position < 0 || position >= 2)
        {
            return new ServiceResponse(
                ServiceResponseCode.FAIL,
                "Invalid tag position");
        }
        
        Room? room = await GetRoomAsync(roomId);

        if (room is null)
        {
            return new ServiceResponse(
                ServiceResponseCode.FAIL,
                "Room not found");
        }

        List<RoomTag> tags = room.RoomTags.ToList();
        
        tags[position].Tag = tag;
        await gameDbContext.SaveChangesAsync();
        
        return new ServiceResponse(ServiceResponseCode.SUCCESS);
    }

    public async Task<ServiceResponse> SaveNewTagAsync(Room room, int position, string tag)
    {
        return await SaveNewTagAsync(room.Id, position, tag); 
    }


    public ServiceResponse AttemptEnterRoom(Room room, User user)
    {
        bool isRoomFull = roomRuntimeService.GetPlayers(room.Id)
            .Count >= room.PlayersLimit;
        bool isBanned = room.BannedPlayers.Contains(user);

        if (isRoomFull)
        {
            return new(
                ServiceResponseCode.FAIL, 
                "This room is full.");
        }

        if (isBanned)
        {
            return new(
                ServiceResponseCode.FAIL,
                "You are banned from this room.");
        }

        string? currentRoomId = userRuntimeService.GetCurrentRoom(user.Id);

        if (currentRoomId is not null)
        {
            roomRuntimeService.RemovePlayer(currentRoomId, user.Id);
            userRuntimeService.ClearCurrentRoom(user.Id);
        }
        
        // TODO: implement user's visits history
        userRuntimeService.SetCurrentRoom(user.Id, room.Id);
        roomRuntimeService.AddPlayer(room.Id, user.Id);

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
                
        PublicNavigatorRoomResponseModel roomResponseModel =new PublicNavigatorRoomResponseModel(
            room.Id,
            room.Name,
            room.Description,
            roomRuntimeService.GetPlayers(room.Id).Count,
            room.PlayersLimit,
            room.Template,
            group,
            room.OwnerId,
            room.Tags,
            room.IsPublic);
        
        return new(
            Code: ServiceResponseCode.SUCCESS,
            Props: new() { roomResponseModel });
    }

    public ServiceResponse GoToHotelView(User user)
    {
        string? currentRoomId = userRuntimeService.GetCurrentRoom(user.Id);

        if (currentRoomId is not null)
        {
            userRuntimeService.ClearCurrentRoom(user.Id);
            roomRuntimeService.RemovePlayer(currentRoomId, user.Id);
        }

        return new(ServiceResponseCode.SUCCESS);
    }
}