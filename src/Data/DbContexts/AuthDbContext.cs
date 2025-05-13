using Iso.Data.Models;
using Iso.Data.Models.RoomModel;
using Iso.Data.Models.UserModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Iso.Data.DbContexts;

public class AuthDbContext : IdentityDbContext<User>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>(entity =>
        { });

        builder.Entity<Friendship>(entity =>
        {
            entity.HasKey(f => new { f.UserId, f.FriendId });
            
            entity.HasOne(f => f.User)
                .WithMany(u => u.UserFriends)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(f => f.Friend)
                .WithMany(u => u.UserFriendOf)
                .HasForeignKey(f => f.FriendId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
};