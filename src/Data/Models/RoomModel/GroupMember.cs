using System.ComponentModel.DataAnnotations.Schema;

namespace Iso.Data.Models.RoomModel;

public class GroupMember
{
    public string GroupId { get; set; }
    public string UserId { get; set; }
    
    [ForeignKey(nameof(GroupId))]
    [InverseProperty(nameof(Group.GroupMembers))]
    public Group Group { get; set; }
}