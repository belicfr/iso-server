namespace Iso.Data.Models.RoomModel;

public class GroupMember
{
    public string GroupId { get; set; }
    public string UserId { get; set; }
    
    public Group Group { get; set; }
}