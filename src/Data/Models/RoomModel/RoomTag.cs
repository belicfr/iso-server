using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Iso.Data.Models.RoomModel;

public class RoomTag
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(10)]
    public string Tag { get; set; }

    [Required]
    public string RoomId { get; set; }

    [ForeignKey(nameof(RoomId))]
    [InverseProperty(nameof(Room.RoomTags))]
    public Room Room { get; set; }
}