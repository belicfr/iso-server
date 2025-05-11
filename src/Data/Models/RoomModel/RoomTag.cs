namespace Iso.Data.Models.RoomModel;

public class RoomTag
{
    public string RoomId { get; set; }
    public string Tag { get; set; }
    
    public Room Room { get; set; }
}