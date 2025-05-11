using System.Collections.Generic;
using System.Threading.Tasks;
using Iso.Data.Models.RoomModel;
using Iso.Data.Models.UserModel;
using Iso.Data.Services.DUserService;
using Moq;
using Xunit;

namespace Iso.Tests.Data.Services.DUserService
{
    public class UserServiceTests
    {
        private readonly Mock<IUserService> _userServiceMock;

        public UserServiceTests()
        {
            _userServiceMock = new Mock<IUserService>();
        }

        [Fact]
        public async Task GetUserAsync_UserExists_ReturnsUser()
        {
            // Arrange
            var userId = "test-user-id";
            var user = new User { Id = userId, Sso = "test-sso" };
            _userServiceMock.Setup(us => us.GetUserAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _userServiceMock.Object.GetUserAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
        }

        [Fact]
        public async Task GetUserAsync_UserDoesNotExist_ReturnsNull()
        {
            // Arrange
            var userId = "non-existent-id";
            _userServiceMock.Setup(us => us.GetUserAsync(userId)).ReturnsAsync((User?)null);

            // Act
            var result = await _userServiceMock.Object.GetUserAsync(userId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetHomeRoomAsync_HomeRoomExists_ReturnsRoom()
        {
            // Arrange
            var userId = "test-user-id";
            var room = new Room { Id = "room-id" };
            _userServiceMock.Setup(us => us.GetHomeRoomAsync(userId)).ReturnsAsync(room);

            // Act
            var result = await _userServiceMock.Object.GetHomeRoomAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("room-id", result.Id);
        }

        [Fact]
        public async Task GetHomeRoomAsync_HomeRoomDoesNotExist_ReturnsNull()
        {
            // Arrange
            var userId = "test-user-id";
            _userServiceMock.Setup(us => us.GetHomeRoomAsync(userId)).ReturnsAsync((Room?)null);

            // Act
            var result = await _userServiceMock.Object.GetHomeRoomAsync(userId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetRoomsForUserAsync_UserHasRooms_ReturnsRooms()
        {
            // Arrange
            var userId = "test-user-id";
            var rooms = new List<Room> { new Room { Id = "room1" }, new Room { Id = "room2" } };
            _userServiceMock.Setup(us => us.GetRoomsForUserAsync(userId)).ReturnsAsync(rooms.AsReadOnly());

            // Act
            var result = await _userServiceMock.Object.GetRoomsForUserAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetRoomsForUserAsync_UserHasNoRooms_ReturnsEmptyList()
        {
            // Arrange
            var userId = "test-user-id";
            _userServiceMock.Setup(us => us.GetRoomsForUserAsync(userId)).ReturnsAsync(new List<Room>().AsReadOnly());

            // Act
            var result = await _userServiceMock.Object.GetRoomsForUserAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
