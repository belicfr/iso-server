using Iso.Data.DbContexts;
using Iso.Data.Models.CreationModels;
using Iso.Data.Models.EventDispatchers.Rooms;
using Iso.Data.Models.RoomModel;
using Iso.Data.Models.UserModel;
using Iso.Data.Services.Runtime.Rooms.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Iso.Data.Services.Runtime.Rooms;

public partial class RoomRuntimeService(
    IDbContextFactory<GameDbContext> gameDbContext): IRoomRuntimeService
{
    private readonly HashSet<Room> _rooms = new();

    public async Task<Room?> GetRoomByIdAsync(string roomId)
    {
        Room? room = _rooms
            .FirstOrDefault(r => r.Id == roomId);

        if (room is not null)
        {
            return room;
        }
        
        GameDbContext context = await gameDbContext.CreateDbContextAsync();
        
        room = await context.Rooms
            .Include(r => r.RoomBans)
            .Include(r => r.RoomRights)
            .Include(r => r.RoomBannedWords)
            .FirstOrDefaultAsync(r => r.Id == roomId);

        if (room is not null)
        {
            _rooms.Add(room);
        }
        
        return room;
    }


    public async Task<HashSet<Room>> GetAllPublicRoomsAsync()
    {
        IOrderedQueryable<Room> query = await InitializeRoomQuery();
        
        HashSet<Room> rooms = await query
            .Where(r => r.IsPublic)
            .ToHashSetAsync();

        foreach (Room room in rooms)
        {
            _rooms.Add(room);
        }

        return rooms;
    }

    public async Task<HashSet<Room>> GetAllRoomsAsync()
    {
        IOrderedQueryable<Room> query = await InitializeRoomQuery();
        
        HashSet<Room> rooms = await query
            .ToHashSetAsync();

        foreach (Room room in rooms)
        {
            _rooms.Add(room);
        }

        return rooms;
    }

    public async Task<HashSet<Room>> GetAllPlayerRoomsAsync(User user)
    {
        return await GetAllPlayerRoomsAsync(user.Id);
        
    }

    public async Task<HashSet<Room>> GetAllPlayerRoomsAsync(string userId)
    {
        IOrderedQueryable<Room> query = await InitializeRoomQuery();
        
        HashSet<Room> rooms = await query
            .Where(r => r.OwnerId == userId)
            .ToHashSetAsync();

        foreach (Room room in rooms)
        {
            _rooms.Add(room);
        }

        return rooms;
    }


    public async Task CreateRoomAsync(Room room)
    {
        _rooms.Add(room);
    }


    /// <summary>
    /// Returns an initialized queryable object
    /// to compose a room selection query.
    /// </summary>
    /// <returns></returns>
    private async Task<IOrderedQueryable<Room>> InitializeRoomQuery()
    {
        GameDbContext context = await gameDbContext.CreateDbContextAsync();
        
        return context.Rooms
            .Include(r => r.RoomBans)
            .Include(r => r.RoomRights)
            .Include(r => r.RoomBannedWords)
            .Take(100)
            .OrderByDescending(r => r.CreatedAt);
    }
}