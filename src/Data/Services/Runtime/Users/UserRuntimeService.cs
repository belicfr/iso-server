using Iso.Data.DbContexts;
using Iso.Data.Models.UserModel;
using Iso.Data.Services.Runtime.Users.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Iso.Data.Services.Runtime.Users;

public partial class UserRuntimeService(
    IDbContextFactory<AuthDbContext> authDbContext): IUserRuntimeService
{
    private readonly HashSet<User> _users = new();
    
    
    public async Task<User?> GetUserByIdAsync(string userId)
    {
        User? user = _users
            .FirstOrDefault(u => u.Id == userId);

        if (user is not null)
        {
            return user;
        }

        AuthDbContext context = await authDbContext.CreateDbContextAsync();
        
        user = await context.Users
            .Include(r => r.Friends)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is not null)
        {
            _users.Add(user);
        }
        
        return user;
    }

    
    public async Task<User?> GetUserBySsoAsync(string sso)
    {
        User? user = _users
            .FirstOrDefault(u => u.Sso == sso);

        if (user is not null)
        {
            return user;
        }

        AuthDbContext context = await authDbContext.CreateDbContextAsync();
        
        user = await context.Users
            .Include(r => r.Friends)
            .FirstOrDefaultAsync(u => u.Sso == sso);

        if (user is not null)
        {
            _users.Add(user);
        }
        
        return user;
    }
}