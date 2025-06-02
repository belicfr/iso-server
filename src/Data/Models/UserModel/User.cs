using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Iso.Data.Models.RoomModel;
using System.Collections.Generic;
using Iso.Shared.Physic;

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
    
    public List<User> Friends { get; set; } = new();

    [NotMapped]
    public Coord2D Position { get; set; } = new(0f, 0f);
}