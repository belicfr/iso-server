namespace Iso.Data.Services.DRoomService;

public class RoomRuntimeService: IRoomRuntimeService
{
    private readonly Dictionary<string, HashSet<string>> _playersInRoom = new();

    public List<string> GetPlayers(string roomId)
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

    public void AddPlayer(string roomId, string userId)
    {
        if (!_playersInRoom.ContainsKey(roomId))
        {
            _playersInRoom[roomId] = new();
        }
        
        _playersInRoom[roomId].Add(userId);
    }

    public void RemovePlayer(string roomId, string userId)
    {
        if (_playersInRoom.TryGetValue(roomId, out var players))
        {
            players.Remove(userId);

            if (players.Count == 0)
            {
                _playersInRoom.Remove(roomId);
            }
        }
    }

    public bool IsPlayerInRoom(string roomId, string userId)
    {
        return _playersInRoom.TryGetValue(roomId, out var players)
            && players.Contains(userId);
    }
}