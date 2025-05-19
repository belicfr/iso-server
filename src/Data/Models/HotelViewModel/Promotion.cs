using System.ComponentModel.DataAnnotations;

namespace Iso.Data.Models.HotelViewModel;

public class Promotion
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public string Title { get; set; }

    [Required]
    public string ThumbnailPath { get; set; }
    
    [Required]
    public string Action { get; set; }
    
    [Required]
    [Range(1, 5)]
    public int Position { get; set; }
}