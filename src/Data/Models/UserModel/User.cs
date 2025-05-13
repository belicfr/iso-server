using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Iso.Data.Models.RoomModel;

namespace Iso.Data.Models.UserModel;

public class User: IdentityUser
{
    [Required]
    [MaxLength(256)]
    public string Sso { get; set; }
        = string.Empty;

    [Required]
    public int Crowns { get; set; } = 0;
    
    public string? HomeRoomId { get; set; }
    
    [NotMapped]
    public Room? HomeRoom { get; set; }
}