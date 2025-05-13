using System.ComponentModel.DataAnnotations;

namespace Iso.Data.Models.RoomModel;

public class RoomTemplate
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } 
    
    [Required]
    public string Template { get; set; }
}