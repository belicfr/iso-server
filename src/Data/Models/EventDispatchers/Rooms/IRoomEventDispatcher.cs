using Iso.Data.Models.RoomModel;

namespace Iso.Data.Models.EventDispatchers.Rooms;

public interface IRoomEventDispatcher
{
    /// <summary>
    /// Notifies all players currently in updated room,
    /// giving new room's information.
    /// </summary>
    /// <param name="room"></param>
    /// <returns></returns>
    Task NotifyRoomUpdatedAsync(Room room);
}