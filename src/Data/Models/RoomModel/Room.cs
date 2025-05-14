using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Iso.Data.Models.UserModel;

namespace Iso.Data.Models.RoomModel;

public class Room
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    
    [Required]
    [MaxLength(1024)]
    public string Description { get; set; }
    
    [Required]
    public string OwnerId { get; set; }
    
    [NotMapped]
    public User Owner { get; set; }

    [MaxLength(10)]
    public string? TagOne { get; set; } = null;
    
    [MaxLength(10)]
    public string? TagTwo { get; set; } = null;

    [Range(1, 200)]
    public int PlayersLimit { get; set; }
    
    
    [Required]
    public string Template { get; set; }
    
    [InverseProperty(nameof(RoomBan.Room))]
    public ICollection<RoomBan> RoomBans { get; set; } = new HashSet<RoomBan>();
    
    [NotMapped]
    public List<User> BannedPlayers { get; set; } = new();
    
    [InverseProperty(nameof(RoomRight.Room))]
    public ICollection<RoomRight> RoomRights { get; set; } = new HashSet<RoomRight>();
    
    [NotMapped]
    public List<User> PlayersWithRights { get; set; } = new();
    
    [InverseProperty(nameof(RoomBannedWord.Room))]
    public ICollection<RoomBannedWord> RoomBannedWords { get; set; } = new HashSet<RoomBannedWord>();
    
    [NotMapped]
    public List<string> BannedWords { get; set; } = new();
    
    [InverseProperty(nameof(Group.Room))]
    public Group? Group { get; set; }

    [Required]
    public bool IsPublic { get; set; }
}
