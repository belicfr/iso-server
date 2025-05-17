using Iso.Data.Models.RoomModel;
using Iso.Data.Models.UserModel;
using Microsoft.EntityFrameworkCore;

namespace Iso.Data.DbContexts;

public class GameDbContext(
    DbContextOptions<GameDbContext> options): DbContext(options)
{
    public DbSet<Room> Rooms { get; set; }
    public DbSet<RoomTemplate> RoomTemplates { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<RoomBan> RoomBans { get; set; }
    public DbSet<RoomRight> RoomRights { get; set; }
    public DbSet<RoomBannedWord> RoomBannedWords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // ROOMS
        modelBuilder.Entity<Room>(entity =>
        { });
        
        // ROOMS TEMPLATES
        modelBuilder.Entity<RoomTemplate>(entity =>
        { });
        
        // ROOMS GROUPS
        modelBuilder.Entity<Group>(entity =>
        { });
        
        // ROOMS BANS
        modelBuilder.Entity<RoomBan>(entity =>
        {
            entity.HasKey(rb => new { rb.RoomId, rb.UserId });
            
            entity.HasOne(rb => rb.Room)
                .WithMany(r => r.RoomBans)
                .HasForeignKey(rb => rb.RoomId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // ROOMS RIGHTS
        modelBuilder.Entity<RoomRight>(entity =>
        {
            entity.HasKey(rr => new { rr.RoomId, rr.UserId });
            
            entity.HasOne(rr => rr.Room)
                .WithMany(r => r.RoomRights)
                .HasForeignKey(rr => rr.RoomId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // ROOM BANNED WORDS
        modelBuilder.Entity<RoomBannedWord>(entity =>
        {
            entity.HasKey(e => new { e.RoomId, e.BannedWord });
            
            entity.HasOne(bw => bw.Room)
                .WithMany(r => r.RoomBannedWords)
                .HasForeignKey(bw => bw.RoomId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // ROOM GROUPS MEMBERS
        modelBuilder.Entity<GroupMember>(entity =>
        {
            entity.HasKey(e => new { e.GroupId, e.UserId });
            
            entity.HasOne(e => e.Group)
                .WithMany(r => r.GroupMembers)
                .HasForeignKey(e => e.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}