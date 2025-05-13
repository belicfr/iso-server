using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Iso.Data.Models.UserModel;

namespace Iso.Data.Models.RoomModel;

public class Group
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
    
    [Required]
    public string RoomId { get; set; }
    
    [ForeignKey(nameof(RoomId))]
    [InverseProperty(nameof(Room.Group))]
    public Room Room { get; set; }
    
    [InverseProperty(nameof(GroupMember.Group))]
    public IEnumerable<GroupMember> GroupMembers { get; set; } = new HashSet<GroupMember>();
    
    [NotMapped]
    public List<User> Members { get; set; } = new();
    
    [Required]
    public GroupMode GroupMode { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; set; } 
}