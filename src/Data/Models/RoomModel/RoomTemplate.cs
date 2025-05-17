using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

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

    [NotMapped]
    public int TilesCount => Regex
        .Matches(Template, "[1-9E]")
        .Count;
}