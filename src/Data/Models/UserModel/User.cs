using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Iso.Data.Models.RoomModel;

namespace Iso.Data.Models.UserModel;

public class User: IdentityUser
{
    public string Sso { get; set; }
        = string.Empty;
    
    
    [NotMapped]
    public List<Room> Rooms { get; set; } = new();

    
    public string? HomeRoomId { get; set; }
    
    [NotMapped]
    public Room? HomeRoom { get; set; }
}