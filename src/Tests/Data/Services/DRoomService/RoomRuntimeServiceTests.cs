using Iso.Data.Services.Runtime.Rooms;

namespace Iso.Tests.Data.Services.DRoomService
{
    public class RoomRuntimeServiceTests
    {
        private readonly RoomRuntimeService _service;

        public RoomRuntimeServiceTests()
        {
            _service = new RoomRuntimeService();
        }

        [Fact]
        public void AddPlayer_ShouldStoreUserIdInCorrectRoom()
        {
            // Arrange
            string roomId = "room1";
            string userId = "user1";

            // Act
            _service.AddPlayer(roomId, userId);

            // Assert
            var userIds = _service.GetPlayers(roomId);
            Assert.Contains(userId, userIds);
        }

        [Fact]
        public void RemovePlayer_ShouldRemoveUserIdFromRoom()
        {
            // Arrange
            string roomId = "room1";
            string userId = "user1";
            _service.AddPlayer(roomId, userId);

            // Act
            _service.RemovePlayer(roomId, userId);

            // Assert
            var userIds = _service.GetPlayers(roomId);
            Assert.DoesNotContain(userId, userIds);
        }

        [Fact]
        public void GetPlayers_ShouldReturnEmptyList_WhenRoomHasNoPlayers()
        {
            // Arrange
            string roomId = "room1";

            // Act
            var result = _service.GetPlayers(roomId);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void IsPlayerInRoom_ShouldReturnTrue_IfPlayerExists()
        {
            // Arrange
            string roomId = "room1";
            string userId = "user1";
            _service.AddPlayer(roomId, userId);

            // Act
            bool isInRoom = _service.IsPlayerInRoom(roomId, userId);

            // Assert
            Assert.True(isInRoom);
        }

        [Fact]
        public void IsPlayerInRoom_ShouldReturnFalse_IfPlayerDoesNotExist()
        {
            // Arrange
            string roomId = "room1";
            string userId = "user1";

            // Act
            bool isInRoom = _service.IsPlayerInRoom(roomId, userId);

            // Assert
            Assert.False(isInRoom);
        }

        [Fact]
        public void GetPlayersCount_ShouldReturnCorrectCount()
        {
            // Arrange
            string roomId = "room1";
            _service.AddPlayer(roomId, "user1");
            _service.AddPlayer(roomId, "user2");

            // Act
            int count = _service.GetPlayersCount(roomId);

            // Assert
            Assert.Equal(2, count);
        }

        [Fact]
        public void RemovePlayer_ShouldCleanupEmptyRoom()
        {
            // Arrange
            string roomId = "room1";
            string userId = "user1";
            _service.AddPlayer(roomId, userId);

            // Act
            _service.RemovePlayer(roomId, userId);
            int count = _service.GetPlayersCount(roomId);

            // Assert
            Assert.Equal(0, count);
        }
    }
}
