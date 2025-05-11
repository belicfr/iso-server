namespace Iso.Data.Models.RoomModel;

public class RoomBan
{
    public string RoomId { get; set; }
    public string UserId { get; set; }
    
    public Room Room { get; set; }
}