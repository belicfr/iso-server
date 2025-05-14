using Iso.Data.DbContexts;
using Iso.Data.Models.RoomModel;
using Iso.Data.Models.UserModel;
using Microsoft.EntityFrameworkCore;

namespace Iso.Data.Services.DUserService;

public class UserService(
    AuthDbContext authDbContext,
    GameDbContext gameDbContext): IUserService
{
    public async Task<User?> GetUserAsync(string userId)
    {
        User? user = await authDbContext.Users
            .AsNoTracking()
            .Include(u => u.Friends)
            .FirstOrDefaultAsync(u => u.Id == userId);

        return user ?? null;
    }

    public async Task<User?> GetUserBySsoAsync(string sso)
    {
        User? user = await authDbContext.Users
            .AsNoTracking()
            .Include(u => u.Friends)
            .FirstOrDefaultAsync(u => u.Sso == sso);
        
        return user ?? null;
    }

    public async Task<Room?> GetHomeRoomAsync(string userId)
    {
        string? homeRoomId = await authDbContext.Users
            .Where(u => u.Id == userId)
            .Select(u => u.HomeRoomId)
            .FirstOrDefaultAsync();

        if (homeRoomId is null)
        {
            return null;
        }
        
        return await gameDbContext.Rooms
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == homeRoomId);
    }

    public async Task<IReadOnlyList<Room>> GetRoomsForUserAsync(string userId)
    {
        return await gameDbContext.Rooms
            .AsNoTracking()
            .Where(r => r.OwnerId == userId)
            // .Skip((page - 1) * pageSize)    // TODO: skip to create pagination
            // .Take(pageSize)
            .ToListAsync();
    }
}