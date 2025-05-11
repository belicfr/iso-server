using System.ComponentModel.DataAnnotations.Schema;
using Iso.Data.Models.UserModel;

namespace Iso.Data.Models.RoomModel;

public class Group
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    
    public string OwnerId { get; set; }
    
    [NotMapped]
    public User Owner { get; set; }
    
    
    public string RoomId { get; set; }
    
    public Room Room { get; set; }
    
    
    public IEnumerable<GroupMember> GroupMembers { get; set; } = new HashSet<GroupMember>();
    
    [NotMapped]
    public List<User> Members { get; set; } = new();
    
    
    public GroupMode GroupMode { get; set; }
    
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}