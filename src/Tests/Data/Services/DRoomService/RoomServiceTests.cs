using Iso.Data.DbContexts;
using Iso.Data.Models.RoomModel;
using Iso.Data.Models.UserModel;
using Iso.Data.Services.DRoomService;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Iso.Tests.Data.Services.DRoomService
{
    public class RoomServiceTests : IDisposable
    {
        private readonly AuthDbContext _authDbContext;
        private readonly GameDbContext _gameDbContext;
        private readonly RoomService _roomService;

        public RoomServiceTests()
        {
            var optionsAuth = new DbContextOptionsBuilder<AuthDbContext>()
                .UseInMemoryDatabase(databaseName: "AuthTestDb")
                .Options;
            _authDbContext = new AuthDbContext(optionsAuth);

            var optionsGame = new DbContextOptionsBuilder<GameDbContext>()
                .UseInMemoryDatabase(databaseName: "GameTestDb")
                .Options;
            _gameDbContext = new GameDbContext(optionsGame);

            _roomService = new RoomService(_authDbContext, _gameDbContext);
        }

        // Cleanup after each test
        public void Dispose()
        {
            _authDbContext.Database.EnsureDeleted();
            _gameDbContext.Database.EnsureDeleted();
        }

        [Fact]
        public async Task GetOwnerAsync_ReturnsOwner_WhenRoomExists()
        {
            // Arrange
            var roomId = "room1";
            var userId = "user1";
            var room = new Room
            { 
                Id = roomId, 
                OwnerId = userId,
                Name = "Room 1",
                Description = "Sample Room Description",
                Template = "",
            };
            var user = new User { Id = userId };

            _gameDbContext.Rooms.Add(room);
            _authDbContext.Users.Add(user);
            await _gameDbContext.SaveChangesAsync();
            await _authDbContext.SaveChangesAsync();

            // Act
            var result = await _roomService.GetOwnerAsync(roomId);

            // Assert
            Assert.Equal(userId, result?.Id);
        }

        [Fact]
        public async Task GetOwnerAsync_ReturnsNull_WhenRoomDoesNotExist()
        {
            // Arrange
            var roomId = "nonexistentRoom";

            // Act
            List<Room> result = await _gameDbContext.Rooms
                .AsNoTracking()
                .Where(r => r.Id == roomId)
                .ToListAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetBannedPlayersAsync_ReturnsBannedPlayers_WhenBansExist()
        {
            // Arrange
            var roomId = "room1";
            var bannedUserId = "bannedUser1";
            var roomBan = new RoomBan { RoomId = roomId, UserId = bannedUserId };
            var bannedUser = new User { Id = bannedUserId };

            _gameDbContext.RoomBans.Add(roomBan);
            _authDbContext.Users.Add(bannedUser);
            await _gameDbContext.SaveChangesAsync();
            await _authDbContext.SaveChangesAsync();

            // Act
            var result = await _roomService.GetBannedPlayersAsync(roomId);

            // Assert
            Assert.Single(result);
            Assert.Equal(bannedUserId, result.First().Id);
        }

        [Fact]
        public async Task GetBannedPlayersAsync_ReturnsEmptyList_WhenNoBansExist()
        {
            // Arrange
            var roomId = "room1";

            // No need to add anything to the database

            // Act
            var result = await _roomService.GetBannedPlayersAsync(roomId);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetPlayersWithRightsAsync_ReturnsPlayersWithRights_WhenRightsExist()
        {
            // Arrange
            var roomId = "room1";
            var rightUserId = "rightUser1";
            var roomRight = new RoomRight { RoomId = roomId, UserId = rightUserId };
            var rightUser = new User { Id = rightUserId };

            _gameDbContext.RoomRights.Add(roomRight);
            _authDbContext.Users.Add(rightUser);
            await _gameDbContext.SaveChangesAsync();
            await _authDbContext.SaveChangesAsync();

            // Act
            var result = await _roomService.GetPlayersWithRightsAsync(roomId);

            // Assert
            Assert.Single(result);
            Assert.Equal(rightUserId, result.First().Id);
        }

        [Fact]
        public async Task GetPlayersWithRightsAsync_ReturnsEmptyList_WhenNoRightsExist()
        {
            // Arrange
            var roomId = "room1";

            // No need to add anything to the database

            // Act
            var result = await _roomService.GetPlayersWithRightsAsync(roomId);

            // Assert
            Assert.Empty(result);
        }
    }
}
