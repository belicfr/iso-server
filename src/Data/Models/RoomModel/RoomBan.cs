using System.ComponentModel.DataAnnotations.Schema;

namespace Iso.Data.Models.RoomModel;

public class RoomBan
{
    public string RoomId { get; set; }
    public string UserId { get; set; }
    
    [ForeignKey(nameof(RoomId))]
    [InverseProperty(nameof(Room.RoomBans))]
    public Room Room { get; set; }
}