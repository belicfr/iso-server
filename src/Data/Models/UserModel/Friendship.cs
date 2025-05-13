using System.ComponentModel.DataAnnotations.Schema;

namespace Iso.Data.Models.UserModel;

public class Friendship
{
    public string UserId { get; set; }
    
    public string FriendId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
    
    [ForeignKey(nameof(FriendId))]
    public User Friend { get; set; }
}