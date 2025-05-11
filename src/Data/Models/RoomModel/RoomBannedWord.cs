namespace Iso.Data.Models.RoomModel;

public class RoomBannedWord
{
    public string RoomId { get; set; }
    public string BannedWord { get; set; }
    
    public Room Room { get; set; }
}