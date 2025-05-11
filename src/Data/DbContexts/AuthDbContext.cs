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
        {
            entity
                .Property(u => u.Sso)
                .HasMaxLength(256)
                .IsRequired();

            entity.Property(u => u.HomeRoomId);
        });
    }
};