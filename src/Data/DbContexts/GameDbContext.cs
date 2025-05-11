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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ROOMS
        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(e => e.Description)
                .HasMaxLength(1024);

            entity.HasMany<RoomBan>(r => r.RoomBans)
                .WithOne(e => e.Room);
            
            entity.HasMany<RoomRight>(r => r.RoomRights)
                .WithOne(e => e.Room);
            
            entity.HasMany<RoomTag>(r => r.RoomTags)
                .WithOne(e => e.Room);
            
            entity.HasMany<RoomBannedWord>(r => r.RoomBannedWords)
                .WithOne(e => e.Room);
        });
        
        // ROOMS TEMPLATES
        modelBuilder.Entity<RoomTemplate>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Template)
                .IsRequired();
        });
        
        // ROOMS GROUPS
        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(1024);
            
            entity.HasOne<Room>(e => e.Room)
                .WithOne(e => e.Group)
                .HasForeignKey<Room>(e => e.Id);
            
            entity.Property(e => e.GroupMode)
                .IsRequired();
            
            entity.Property(e => e.CreatedAt)
                .IsRequired();
        });
        
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
            entity.HasKey(bw => bw.RoomId);
            
            entity.Property(rr => rr.BannedWord)
                .IsRequired()
                .HasMaxLength(75);
            
            entity.HasOne(bw => bw.Room)
                .WithMany(r => r.RoomBannedWords)
                .HasForeignKey(bw => bw.RoomId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // ROOM TAGS
        modelBuilder.Entity<RoomTag>(entity =>
        {
            entity.HasKey(e => e.RoomId);
            
            entity.Property(rt => rt.Tag)
                .IsRequired()
                .HasMaxLength(10);
            
            entity.HasOne(rt => rt.Room)
                .WithMany(r => r.RoomTags)
                .HasForeignKey(rt => rt.RoomId)
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