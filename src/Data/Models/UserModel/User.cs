using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Iso.Data.Models.RoomModel;
using System.Collections.Generic;

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
    
    [InverseProperty(nameof(Friendship.User))]
    public ICollection<Friendship> UserFriends { get; set; }
    
    [InverseProperty(nameof(Friendship.Friend))]
    public ICollection<Friendship> UserFriendOf { get; set; }
}
