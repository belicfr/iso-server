using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using Iso.Data.Models.UserModel;

namespace Iso.Data.Models.RoomModel;

public class Room
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public string Name { get; set; }
    public string Description { get; set; }
    
    
    public string OwnerId { get; set; }
    
    [NotMapped]
    public User Owner { get; set; }
    
    
    public IEnumerable<RoomTag> RoomTags { get; set; } = new HashSet<RoomTag>();
    
    [NotMapped]
    public List<string> Tags { get; set; } = new();
    
    
    public int PlayersLimit { get; set; }
    
    
    [NotMapped]
    public List<User> PlayersInRoom { get; set; } = new();
    
    
    public string Template { get; set; }
    
    
    public IEnumerable<RoomBan> RoomBans { get; set; } = new HashSet<RoomBan>();
    
    public IEnumerable<RoomRight> RoomRights { get; set; } = new HashSet<RoomRight>();
    
    
    [NotMapped]
    public List<User> BannedPlayers { get; set; } = new();
    
    [NotMapped]
    public List<User> PlayersWithRights { get; set; } = new();
    
    
    public IEnumerable<RoomBannedWord> RoomBannedWords { get; set; } = new HashSet<RoomBannedWord>();
    
    [NotMapped]
    public List<string> BannedWords { get; set; } = new();
    
    
    public string? GroupId { get; set; }
    
    public Group? Group { get; set; }
    
    
    public bool IsPublic { get; set; }
}