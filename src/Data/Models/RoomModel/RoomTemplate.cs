namespace Iso.Data.Models.RoomModel;

public class RoomTemplate
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public string Name { get; set; } 
    
    public string Template { get; set; }
}