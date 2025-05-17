using Iso.Data.Models;
using Iso.Data.Models.RoomModel;
using Iso.Data.Models.UserModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Iso.Data.DbContexts;

public class AuthDbContext : IdentityDbContext<User>
{
    public DbSet<Friendship> Friendships { get; set; }
    
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>()
            .ToTable("AspNetUsers")
            .HasMany(u => u.Friends)
            .WithMany()
            .UsingEntity<Friendship>(
                j => j.HasOne(f => f.Friend)
                    .WithMany()
                    .HasForeignKey(f => f.FriendId)
                    .OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne(f => f.User)
                    .WithMany()
                    .HasForeignKey(f => f.UserId)
                    .OnDelete(DeleteBehavior.Cascade),
                j => j.HasKey(f => new { f.UserId, f.FriendId }));
    }
};