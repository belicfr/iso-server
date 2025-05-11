using Iso.Data.DbContexts;
using Iso.Data.Models.RoomModel;
using Iso.Data.Models.UserModel;
using Microsoft.EntityFrameworkCore;

namespace Iso.Data.Services.DRoomService;

public class RoomService(
    AuthDbContext authDbContext,
    GameDbContext gameDbContext): IRoomService
{
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
}