using Iso.Data.DbContexts;
using Iso.Data.Models.RoomModel;
using Iso.Data.Models.UserModel;
using Iso.Data.Services.Runtime.Rooms;
using Iso.Data.Services.Runtime.Rooms.Interfaces;
using Iso.Data.Services.Runtime.Users;
using Microsoft.EntityFrameworkCore;

namespace Iso.Data.Services.DUserService;

public class UserService(
    AuthDbContext authDbContext,
    GameDbContext gameDbContext,
    UserRuntimeService userRuntimeService,
    IRoomRuntimeService roomRuntimeService): IUserService
{
    public async Task<User?> GetUserAsync(string userId)
    {
        return await userRuntimeService.GetUserByIdAsync(userId);
    }

    public async Task<User?> GetUserBySsoAsync(string sso)
    {
        return await userRuntimeService.GetUserBySsoAsync(sso);
    }

    public async Task<Room?> GetHomeRoomAsync(string userId)
    {
        User? user = await GetUserAsync(userId);

        if (user?.HomeRoomId is null)
        {
            return null;
        }

        return await roomRuntimeService.GetRoomByIdAsync(user.HomeRoomId);
    }

    public async Task<HashSet<Room>> GetRoomsForUserAsync(string userId)
    {
        return await roomRuntimeService.GetAllPlayerRoomsAsync(userId);
    }
}