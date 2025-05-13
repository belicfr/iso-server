using Iso.Data.Services.DUserService;
using Xunit;

namespace Iso.Tests.Data.Services.DUserService
{
    public class UserRuntimeServiceTests
    {
        private readonly UserRuntimeService _service;

        public UserRuntimeServiceTests()
        {
            _service = new UserRuntimeService();
        }

        [Fact]
        public void SetCurrentRoom_ShouldAssignRoomToUser()
        {
            // Arrange
            string userId = "user1";
            string roomId = "roomA";

            // Act
            _service.SetCurrentRoom(userId, roomId);

            // Assert
            Assert.Equal(roomId, _service.GetCurrentRoom(userId));
        }

        [Fact]
        public void ClearCurrentRoom_ShouldRemoveRoomAssociation()
        {
            // Arrange
            string userId = "user1";
            _service.SetCurrentRoom(userId, "roomA");

            // Act
            _service.ClearCurrentRoom(userId);

            // Assert
            Assert.Null(_service.GetCurrentRoom(userId));
            Assert.False(_service.IsInRoom(userId));
        }

        [Fact]
        public void GetCurrentRoom_ShouldReturnNull_WhenUserNotAssigned()
        {
            // Arrange
            string userId = "user1";

            // Act
            string? result = _service.GetCurrentRoom(userId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void IsInRoom_ShouldReturnTrue_WhenUserAssigned()
        {
            // Arrange
            string userId = "user1";
            _service.SetCurrentRoom(userId, "roomA");

            // Act
            bool result = _service.IsInRoom(userId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsInRoom_ShouldReturnFalse_WhenUserNotAssigned()
        {
            // Arrange
            string userId = "user1";

            // Act
            bool result = _service.IsInRoom(userId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SetCurrentRoom_ShouldOverwritePreviousRoom()
        {
            // Arrange
            string userId = "user1";
            _service.SetCurrentRoom(userId, "roomA");

            // Act
            _service.SetCurrentRoom(userId, "roomB");

            // Assert
            Assert.Equal("roomB", _service.GetCurrentRoom(userId));
        }
    }
}
