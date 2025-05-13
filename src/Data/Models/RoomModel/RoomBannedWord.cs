using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Iso.Data.Models.RoomModel;

public class RoomBannedWord
{
    [Key]
    public string RoomId { get; set; }
    
    [Required]
    [MaxLength(75)]
    public string BannedWord { get; set; }
    
    [ForeignKey(nameof(RoomId))]
    [InverseProperty(nameof(Room.RoomBannedWords))]
    public Room Room { get; set; }
}