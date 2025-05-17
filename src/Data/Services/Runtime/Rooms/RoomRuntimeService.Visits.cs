using Iso.Data.Models.UserModel;

namespace Iso.Data.Services.Runtime.Rooms;

public partial class RoomRuntimeService
{
    private readonly Dictionary<string, HashSet<User>> _playersInRoom = new();
    
    public List<User> GetPlayers(string roomId)
    {
        return _playersInRoom.TryGetValue(roomId, out var players)
            ? players.ToList()
            : new();
    }

    public int GetPlayersCount(string roomId)
    {
        return _playersInRoom.TryGetValue(roomId, out var players)
            ? players.Count
            : 0;
    }

    public void AddPlayer(string roomId, User user)
    {
        if (!_playersInRoom.ContainsKey(roomId))
        {
            _playersInRoom[roomId] = new();
        }
        
        _playersInRoom[roomId].Add(user);
    }

    public void RemovePlayer(string roomId, User user)
    {
        if (_playersInRoom.TryGetValue(roomId, out var players))
        {
            players.Remove(user);

            if (players.Count == 0)
            {
                _playersInRoom.Remove(roomId);
            }
        }
    }

    public bool IsPlayerInRoom(string roomId, User user)
    {
        return _playersInRoom.TryGetValue(roomId, out var players)
               && players.Contains(user);
    }
}