using System.ComponentModel.DataAnnotations.Schema;

namespace Iso.Data.Models.RoomModel;

public class RoomRight
{
    public string RoomId { get; set; }
    public string UserId { get; set; }
    
    [ForeignKey(nameof(RoomId))]
    [InverseProperty(nameof(Room.RoomRights))]
    public Room Room { get; set; }
}