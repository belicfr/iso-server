namespace Iso.Data.Services.DUserService;

public class UserRuntimeService: IUserRuntimeService
{
    private readonly Dictionary<string, string> _usersCurrentRoom = new();
    
    public void SetCurrentRoom(string userId, string roomId)
    {
        _usersCurrentRoom[userId] = roomId;
    }

    public void ClearCurrentRoom(string userId)
    {
        _usersCurrentRoom.Remove(userId);
    }

    public string? GetCurrentRoom(string userId)
    {
        return _usersCurrentRoom.TryGetValue(userId, out var room) 
            ? room 
            : null;
    }

    public bool IsInRoom(string userId)
    {
        return _usersCurrentRoom.ContainsKey(userId);
    }
}