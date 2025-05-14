using Iso.Data.DbContexts;
using Iso.Data.Models.RoomModel;
using Iso.Data.Models.UserModel;
using Iso.Data.Services.DRoomService;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iso.Data.Services;
using Iso.Data.Services.DUserService;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Iso.Tests.Data.Services.DRoomService
{
    public class RoomServiceTests : IDisposable
    {
        private readonly AuthDbContext _authDbContext;
        private readonly GameDbContext _gameDbContext;
        private readonly RoomService _roomService;
        private readonly UserService _userService;
        private readonly RoomRuntimeService _roomRuntimeService;
        private readonly UserRuntimeService _userRuntimeService;

        public RoomServiceTests()
        {
            DbContextOptions<AuthDbContext> optionsAuth 
                = new DbContextOptionsBuilder<AuthDbContext>()
                    .UseInMemoryDatabase(databaseName: "AuthTestDb")
                    .Options;
            
            _authDbContext = new AuthDbContext(optionsAuth);

            DbContextOptions<GameDbContext> optionsGame
                = new DbContextOptionsBuilder<GameDbContext>()
                    .UseInMemoryDatabase(databaseName: "GameTestDb")
                    .Options;
            
            _gameDbContext = new GameDbContext(optionsGame);
            
            ServiceCollection services = new ServiceCollection();
            services.AddDbContext<AuthDbContext>(options =>
                options.UseInMemoryDatabase("AuthTestDb"));
            services.AddDbContext<GameDbContext>(options =>
                options.UseInMemoryDatabase("GameTestDb"));
            services.AddScoped<UserService>();
            services.AddSingleton<RoomRuntimeService>();
            services.AddSingleton<UserRuntimeService>();
            
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            _roomRuntimeService 
                = serviceProvider.GetRequiredService<RoomRuntimeService>();
            
            _userRuntimeService 
                = serviceProvider.GetRequiredService<UserRuntimeService>();
            
            _userService 
                = serviceProvider.GetRequiredService<UserService>();

            _roomService = new RoomService(
                _authDbContext, 
                _gameDbContext,
                _userService,
                _roomRuntimeService,
                _userRuntimeService);
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
            string roomId = "room1";
            string userId = "user1";
            Room room = new Room
            { 
                Id = roomId, 
                OwnerId = userId,
                Name = "Room 1",
                Description = "Sample Room Description",
                Template = "",
            };
            User user = new User { Id = userId };

            _gameDbContext.Rooms.Add(room);
            _authDbContext.Users.Add(user);
            await _gameDbContext.SaveChangesAsync();
            await _authDbContext.SaveChangesAsync();

            // Act
            User? result = await _roomService.GetOwnerAsync(roomId);

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

        [Fact]
        public async Task AttemptEnterRoomAsync_Success_WhenRoomHasSpace()
        {
            // Arrange
            var room = new Room
            {
                Id = "r1",
                Name = "Room 1",
                Description = "Sample Room Description",
                OwnerId = "u100",
                Template = "",
                PlayersLimit = 2, 
            };
            
            User user = new User { Id = "u1" };
            
            _gameDbContext.Rooms.Add(room);
            await _gameDbContext.SaveChangesAsync();
            
            // Act
            ServiceResponse result = _roomService.AttemptEnterRoom(room, user);
            
            // Assert
            Assert.Equal(ServiceResponseCode.SUCCESS, result.Code);
            Assert.Contains(
                user.Id, 
                _roomRuntimeService.GetPlayers(room.Id));
            Assert.Equal(room.Id, _userRuntimeService.GetCurrentRoom(user.Id));
        }

        [Fact]
        public async Task AttemptEnterRoomAsync_Fail_WhenRoomIsFull()
        {
            // Arrange
            var room = new Room
            {
                Id = "r1", 
                Name = "Room 1",
                Description = "Sample Room Description",
                OwnerId = "u100",
                Template = "",
                PlayersLimit = 1,
            };
            
            User alreadyInRoomUser = new User { Id = "u1" };
            User user = new User { Id = "u2" };
        
            _gameDbContext.Rooms.Add(room);
            await _gameDbContext.SaveChangesAsync();
            
            _roomRuntimeService.AddPlayer(room.Id, alreadyInRoomUser.Id);
            _userRuntimeService.SetCurrentRoom(alreadyInRoomUser.Id, room.Id);
            
            // Act
            ServiceResponse result = _roomService.AttemptEnterRoom(room, user);
            
            // Assert
            Assert.Equal(ServiceResponseCode.FAIL, result.Code);
            Assert.DoesNotContain(
                user.Id, 
                _roomRuntimeService.GetPlayers(room.Id));
            Assert.Null(_userRuntimeService.GetCurrentRoom(user.Id));
        }

        [Fact]
        public async Task AttemptEnterRoomAsync_Fail_WhenUserIsBanned()
        {
            // Arrange
            var room = new Room
            {
                Id = "r1",
                Name = "Room 1",
                Description = "Sample Room Description",
                OwnerId = "u100",
                Template = "",
                PlayersLimit = 2,
                BannedPlayers = new List<User>()
            };
            var bannedUser = new User { Id = "bannedUser" };
            room.BannedPlayers.Add(bannedUser);
            var user = new User { Id = "user1" };
            _gameDbContext.Rooms.Add(room);
            await _gameDbContext.SaveChangesAsync();
            
            // Act
            var result = _roomService.AttemptEnterRoom(room, bannedUser);
            
            // Assert
            Assert.Equal(ServiceResponseCode.FAIL, result.Code);
            Assert.Contains(bannedUser, room.BannedPlayers);
        }

        [Fact]
        public async Task AttemptEnterRoomAsync_Success_WhenUserIsNotBanned()
        {
            // Arrange
            var room = new Room
            {
                Id = "r1",
                Name = "Room 1",
                Description = "Sample Room Description",
                OwnerId = "u100",
                Template = "",
                PlayersLimit = 2,
                BannedPlayers = new List<User>()
            };
            var user = new User { Id = "user1" };
            _gameDbContext.Rooms.Add(room);
            await _gameDbContext.SaveChangesAsync();
            
            // Act
            var result = _roomService.AttemptEnterRoom(room, user);
            
            // Assert
            Assert.Equal(ServiceResponseCode.SUCCESS, result.Code);
            Assert.Contains(
                user.Id, 
                _roomRuntimeService.GetPlayers(room.Id));
        }

        [Fact]
        public async Task SaveNewNameAsync_Success_WhenRoomExists()
        {
            // Arrange
            var room = new Room 
            {
                Id = "room1", 
                OwnerId = "owner1",
                Name = "Room 1",
                Description = "Sample Room Description",
                Template = "",
            };
            _gameDbContext.Rooms.Add(room);
            await _gameDbContext.SaveChangesAsync();
            
            // Act
            var result = await _roomService.SaveNewNameAsync(room.Id, "New Name");
            
            // Assert
            Assert.Equal(ServiceResponseCode.SUCCESS, result.Code);
            Assert.Equal("New Name", room.Name);
        }

        [Fact]
        public async Task SaveNewNameAsync_Fail_WhenRoomDoesNotExist()
        {
            // Act
            var result = await _roomService.SaveNewNameAsync("nonexistentRoom", "New Name");
            
            // Assert
            Assert.Equal(ServiceResponseCode.FAIL, result.Code);
        }

        [Fact]
        public async Task SaveNewDescriptionAsync_Success_WhenRoomExists()
        {
            // Arrange
            var room = new Room 
            {
                Id = "room1", 
                OwnerId = "owner1",
                Name = "Room 1",
                Description = "Sample Room Description",
                Template = "",
            };
            _gameDbContext.Rooms.Add(room);
            await _gameDbContext.SaveChangesAsync();
            
            // Act
            var result = await _roomService.SaveNewDescriptionAsync(room.Id, "New Description");
            
            // Assert
            Assert.Equal(ServiceResponseCode.SUCCESS, result.Code);
            Assert.Equal("New Description", room.Description);
        }

        [Fact]
        public async Task SaveNewDescriptionAsync_Fail_WhenRoomDoesNotExist()
        {
            // Act
            var result = await _roomService.SaveNewDescriptionAsync("nonexistentRoom", "New Description");
            
            // Assert
            Assert.Equal(ServiceResponseCode.FAIL, result.Code);
        }

        [Fact]
        public async Task AddRoomRightsAsync_Success_WhenRoomAndUserExist()
        {
            // Arrange
            var room = new Room 
            {
                Id = "room1", 
                OwnerId = "owner1",
                Name = "Room 1",
                Description = "Sample Room Description",
                Template = "",
            };
            var user = new User { Id = "user1" };
            _gameDbContext.Rooms.Add(room);
            _authDbContext.Users.Add(user);
            await _gameDbContext.SaveChangesAsync();
            await _authDbContext.SaveChangesAsync();
            
            // Act
            var result = await _roomService.AddRoomRightsAsync(room.Id, user.Id);
            
            // Assert
            Assert.Equal(ServiceResponseCode.SUCCESS, result.Code);
            Assert.Contains(room.RoomRights, r => r.UserId == user.Id);
        }

        [Fact]
        public async Task AddRoomRightsAsync_Fail_WhenRoomDoesNotExist()
        {
            // Act
            var result = await _roomService.AddRoomRightsAsync("nonexistentRoom", "user1");
            
            // Assert
            Assert.Equal(ServiceResponseCode.FAIL, result.Code);
        }

        [Fact]
        public async Task AddRoomRightsAsync_Fail_WhenUserDoesNotExist()
        {
            // Arrange
            var room = new Room 
            {
                Id = "room1", 
                OwnerId = "owner1",
                Name = "Room 1",
                Description = "Sample Room Description",
                Template = "",
            };
            _gameDbContext.Rooms.Add(room);
            await _gameDbContext.SaveChangesAsync();
            
            // Act
            var result = await _roomService.AddRoomRightsAsync(room.Id, "nonexistentUser");
            
            // Assert
            Assert.Equal(ServiceResponseCode.FAIL, result.Code);
        }

        [Fact]
        public async Task AddRoomRightsAsync_Fail_WhenUserIsOwner()
        {
            // Arrange
            var room = new Room 
            {
                Id = "room1", 
                OwnerId = "owner1",
                Name = "Room 1",
                Description = "Sample Room Description",
                Template = "",
            };
            var user = new User { Id = "owner1" };
            _gameDbContext.Rooms.Add(room);
            _authDbContext.Users.Add(user);
            await _gameDbContext.SaveChangesAsync();
            await _authDbContext.SaveChangesAsync();
            
            // Act
            var result = await _roomService.AddRoomRightsAsync(room.Id, user.Id);
            
            // Assert
            Assert.Equal(ServiceResponseCode.FAIL, result.Code);
        }

        [Fact]
        public async Task AddRoomRightsAsync_Fail_WhenUserAlreadyHasRights()
        {
            // Arrange
            var room = new Room 
            {
                Id = "room1", 
                OwnerId = "owner1",
                Name = "Room 1",
                Description = "Sample Room Description",
                Template = "",
            };
            var user = new User { Id = "user1" };
            room.RoomRights.Add(new RoomRight { RoomId = room.Id, UserId = user.Id });
            _gameDbContext.Rooms.Add(room);
            _authDbContext.Users.Add(user);
            await _gameDbContext.SaveChangesAsync();
            await _authDbContext.SaveChangesAsync();
            
            // Act
            var result = await _roomService.AddRoomRightsAsync(room.Id, user.Id);
            
            // Assert
            Assert.Equal(ServiceResponseCode.FAIL, result.Code);
        }

        [Fact]
        public async Task RemoveRoomRightsAsync_Success_WhenRoomAndUserExist()
        {
            // Arrange
            var room = new Room 
            {
                Id = "room1", 
                OwnerId = "owner1",
                Name = "Room 1",
                Description = "Sample Room Description",
                Template = "",
            };
            var user = new User { Id = "user1" };
            room.RoomRights.Add(new RoomRight { RoomId = room.Id, UserId = user.Id });
            _gameDbContext.Rooms.Add(room);
            _authDbContext.Users.Add(user);
            await _gameDbContext.SaveChangesAsync();
            await _authDbContext.SaveChangesAsync();
            
            // Act
            var result = await _roomService.RemoveRoomRightsAsync(room.Id, user.Id);
            
            // Assert
            Assert.Equal(ServiceResponseCode.SUCCESS, result.Code);
            Assert.DoesNotContain(room.RoomRights, r => r.UserId == user.Id);
        }

        [Fact]
        public async Task RemoveRoomRightsAsync_Fail_WhenRoomDoesNotExist()
        {
            // Act
            var result = await _roomService.RemoveRoomRightsAsync("nonexistentRoom", "user1");
            
            // Assert
            Assert.Equal(ServiceResponseCode.FAIL, result.Code);
        }

        [Fact]
        public async Task RemoveRoomRightsAsync_Fail_WhenUserDoesNotExist()
        {
            // Arrange
            var room = new Room 
            {
                Id = "room1", 
                OwnerId = "owner1",
                Name = "Room 1",
                Description = "Sample Room Description",
                Template = "",
            };
            _gameDbContext.Rooms.Add(room);
            await _gameDbContext.SaveChangesAsync();
            
            // Act
            var result = await _roomService.RemoveRoomRightsAsync(room.Id, "nonexistentUser");
            
            // Assert
            Assert.Equal(ServiceResponseCode.FAIL, result.Code);
        }

        [Fact]
        public async Task RemoveRoomRightsAsync_Fail_WhenUserIsOwner()
        {
            // Arrange
            var room = new Room 
            {
                Id = "room1", 
                OwnerId = "owner1",
                Name = "Room 1",
                Description = "Sample Room Description",
                Template = "",
            };
            var user = new User { Id = "owner1" };
            _gameDbContext.Rooms.Add(room);
            _authDbContext.Users.Add(user);
            await _gameDbContext.SaveChangesAsync();
            await _authDbContext.SaveChangesAsync();
            
            // Act
            var result = await _roomService.RemoveRoomRightsAsync(room.Id, user.Id);
            
            // Assert
            Assert.Equal(ServiceResponseCode.FAIL, result.Code);
        }

        [Fact]
        public async Task RemoveRoomRightsAsync_Fail_WhenUserDoesNotHaveRights()
        {
            // Arrange
            var room = new Room 
            {
                Id = "room1", 
                OwnerId = "owner1",
                Name = "Room 1",
                Description = "Sample Room Description",
                Template = "",
            };
            var user = new User { Id = "user1" };
            _gameDbContext.Rooms.Add(room);
            _authDbContext.Users.Add(user);
            await _gameDbContext.SaveChangesAsync();
            await _authDbContext.SaveChangesAsync();
            
            // Act
            var result = await _roomService.RemoveRoomRightsAsync(room.Id, user.Id);
            
            // Assert
            Assert.Equal(ServiceResponseCode.FAIL, result.Code);
        }
    }
}
