using Iso.Data.DbContexts;
using Iso.Data.Models.CreationModels;
using Iso.Data.Models.EventDispatchers.Rooms;
using Iso.Data.Models.RoomModel;
using Iso.Data.Models.UserModel;
using Iso.Data.Services.DRoomService.Responses;
using Iso.Data.Services.DRoomTemplateService;
using Iso.Data.Services.DUserService;
using Iso.Data.Services.Runtime.Rooms;
using Iso.Data.Services.Runtime.Rooms.Interfaces;
using Iso.Data.Services.Runtime.Users;
using Microsoft.AspNetCore.Http;

namespace Iso.Data.Services.DRoomService;

public class RoomService(
    AuthDbContext authDbContext,
    GameDbContext gameDbContext,
    UserService userService,
    RoomTemplateService roomTemplateService,
    IRoomRuntimeService roomRuntimeService,
    UserRuntimeService userRuntimeService,
    IRoomEventDispatcher roomEventDispatcher): IRoomService
{
    public async Task<Room?> CreateAsync(NewRoomCreationModel creationModel, string actorId)
    {
        bool isActorExists = (await userService.GetUserAsync(actorId)) is not null;

        if (!isActorExists
            || string.IsNullOrWhiteSpace(creationModel.Name))
        {
            return null;
        }
        
        RoomTemplate? roomTemplate = await roomTemplateService
            .GetRoomTemplateAsync(creationModel.TemplateId);

        if (roomTemplate is null)
        {
            return null;
        }
        
        string thumbnail = "/src/assets/gamelib/arooms/pholder_roomtnail.png";

        Room room = new()
        {
            Name = creationModel.Name,
            Description = creationModel.Description,
            OwnerId = actorId,
            TagOne = creationModel.TagOne,
            TagTwo = creationModel.TagTwo,
            PlayersLimit = 10,                    // TODO: implement
            Template = roomTemplate.Template,
            Thumbnail = thumbnail,
        };
        
        await roomRuntimeService.CreateRoomAsync(room);
        
        gameDbContext.Rooms
            .Add(room);
        
        await gameDbContext.SaveChangesAsync();

        return room;
    }
    

    public async Task<Room?> GetRoomAsync(string id)
    {
        return await roomRuntimeService
            .GetRoomByIdAsync(id);
    }
    

    public async Task<IEnumerable<Room>> GetPublicRoomsAsync()
    {
        return await roomRuntimeService
            .GetAllPublicRoomsAsync();
    }
    

    public async Task<IEnumerable<Room>> GetAllRoomsAsync()
    {
        return await roomRuntimeService
            .GetAllRoomsAsync();
    }
    

    public async Task<IEnumerable<Room>> GetPlayerRoomsAsync(string userId)
    {
        return await roomRuntimeService
            .GetAllPlayerRoomsAsync(userId);
    }

    public async Task<IEnumerable<Room>> GetPlayerRoomsAsync(User user)
    {
        return await GetPlayerRoomsAsync(user.Id);
    }

    
    public async Task<NewNameResponse> SaveNewNameAsync(string roomId, string actorId, string name)
    {
        if (name.Length is < 1 or > 50)
        {
            return NewNameResponse.NAME_LENGTH;
        }
        
        Room? room = await GetRoomAsync(roomId);

        if (room is null)
        {
            return NewNameResponse.ROOM_NOT_EXISTS;
        }

        if (room.OwnerId != actorId)
        {
            return NewNameResponse.MUST_BE_OWNER;
        }
        
        bool success = await roomRuntimeService.UpdateRoomNameAsync(roomId, name);

        if (!success)
        {
            return NewNameResponse.FAIL;
        }

        gameDbContext
            .Attach(room);
        
        gameDbContext
            .Entry(room)
            .Property(r => r.Name)
            .IsModified = true;
        
        await gameDbContext.SaveChangesAsync();
        
        await roomEventDispatcher
            .NotifyRoomUpdatedAsync(room);
        
        return NewNameResponse.SUCCESS;
    }


    public async Task<NewDescriptionResponse> SaveNewDescriptionAsync(string roomId, string actorId, string description)
    {
        if (description.Length is < 1 or > 1024)
        {
            return NewDescriptionResponse.DESCRIPTION_LENGTH;
        }
        
        Room? room = await GetRoomAsync(roomId);

        if (room is null)
        {
            return NewDescriptionResponse.ROOM_NOT_EXISTS;
        }

        if (room.OwnerId != actorId)
        {
            return NewDescriptionResponse.MUST_BE_OWNER;
        }
        
        bool success = await roomRuntimeService.UpdateRoomDescriptionAsync(roomId, description);

        if (!success)
        {
            return NewDescriptionResponse.FAIL;
        }
        
        gameDbContext
            .Attach(room);
        
        gameDbContext
            .Entry(room)
            .Property(r => r.Description)
            .IsModified = true;
        
        await gameDbContext.SaveChangesAsync();
        
        await roomEventDispatcher
            .NotifyRoomUpdatedAsync(room);
        
        return NewDescriptionResponse.SUCCESS;
    }


    public async Task<NewTagResponse> SaveNewTagAsync(string roomId, string actorId, int position, string tag)
    {
        if (position is < 0 or > 1)
        {
            return NewTagResponse.INVALID_POSITION;
        }

        if (tag.Length is < 1 or > 10)
        {
            return NewTagResponse.TAG_LENGTH;
        }
        
        Room? room = await GetRoomAsync(roomId);

        if (room is null)
        {
            return NewTagResponse.ROOM_NOT_EXISTS;
        }

        if (room.OwnerId != actorId)
        {
            return NewTagResponse.MUST_BE_OWNER;
        }

        bool success = false;
        
        switch (position)
        {
            case 0:
                success = await roomRuntimeService.UpdateRoomTagOneAsync(roomId, tag);
                break;
            
            case 1:
                success = await roomRuntimeService.UpdateRoomTagTwoAsync(roomId, tag);
                break;
        }

        if (!success)
        {
            return NewTagResponse.FAIL;
        }
        
        gameDbContext
            .Attach(room);
        
        gameDbContext
            .Entry(room)
            .Property(r => r.TagOne)
            .IsModified = true;
        
        gameDbContext
            .Entry(room)
            .Property(r => r.TagTwo)
            .IsModified = true;
        
        await gameDbContext.SaveChangesAsync();
        
        await roomEventDispatcher
            .NotifyRoomUpdatedAsync(room);

        return NewTagResponse.SUCCESS;
    }


    public async Task<EnterResponse> AttemptEnterRoom(string roomId, string actorId)
    {
        User? actor = await userService.GetUserAsync(actorId);
        Room? room = await GetRoomAsync(roomId);

        if (actor is null)
        {
            return EnterResponse.FAIL;
        }
        
        if (room is null)
        {
            return EnterResponse.ROOM_NOT_EXISTS;
        }
        
        bool isRoomFull = roomRuntimeService.GetPlayers(roomId)
            .Count >= room.PlayersLimit;
        
        bool isBanned = room.RoomBans
            .Any(b => b.UserId == actor.Id);

        if (isRoomFull)
        {
            return EnterResponse.ROOM_FULL;
        }

        if (isBanned)
        {
            return EnterResponse.USER_BANNED;
        }

        string? currentRoomId = userRuntimeService.GetCurrentRoom(actor.Id);

        if (currentRoomId is not null)
        {
            roomRuntimeService.RemovePlayer(currentRoomId, actor);
            userRuntimeService.ClearCurrentRoom(actor.Id);
        }
        
        // TODO: implement user's visits history
        userRuntimeService.SetCurrentRoom(actor.Id, room.Id);
        roomRuntimeService.AddPlayer(room.Id, actor);
        
        return EnterResponse.SUCCESS;
    }
    

    public async Task<GoToHotelViewResponse> GoToHotelView(string userId)
    {
        User? user = await userService.GetUserAsync(userId);

        if (user is null)
        {
            return GoToHotelViewResponse.FAIL;
        }
        
        string? currentRoomId = userRuntimeService.GetCurrentRoom(userId);

        if (currentRoomId is not null)
        {
            userRuntimeService.ClearCurrentRoom(userId);
            roomRuntimeService.RemovePlayer(currentRoomId, user);
        }

        return GoToHotelViewResponse.SUCCESS;
    }


    public async Task<AddRoomRightsResponse> AddRoomRightsAsync(string roomId, string actorId, string userId)
    {
        // TODO: implement room rights to give rights
        
        Room? room = await GetRoomAsync(roomId);
        
        if (room is null)
        {
            return AddRoomRightsResponse.ROOM_NOT_EXISTS;
        }
        
        User? user = await userService.GetUserAsync(userId);
        
        if (user is null)
        {
            return AddRoomRightsResponse.USER_NOT_EXISTS;
        }

        if (userId == room.OwnerId)
        {
            return AddRoomRightsResponse.USER_IS_OWNER;
        }
        
        bool roomRightAlreadyGiven = room.RoomRights
            .Any(r => r.UserId == userId);
        
        if (roomRightAlreadyGiven)
        {
            return AddRoomRightsResponse.ALREADY_HAS_RIGHTS;
        }

        RoomRight newRight = new()
        {
            RoomId = room.Id,
            UserId = userId,
        };

        bool success = await roomRuntimeService.GiveRoomRightsAsync(
            room.Id,
            newRight);

        if (!success)
        {
            return AddRoomRightsResponse.FAIL;
        }

        gameDbContext.RoomRights
            .Add(newRight);
        
        await gameDbContext.SaveChangesAsync();
        
        return AddRoomRightsResponse.SUCCESS;
    }


    public async Task<RemoveRoomRightsResponse> RemoveRoomRightsAsync(string roomId, string actorId, string userId)
    {
        // TODO: implement room rights to remove rights
        
        Room? room = await GetRoomAsync(roomId);
        
        if (room is null)
        {
            return RemoveRoomRightsResponse.ROOM_NOT_EXISTS;
        }
        
        User? user = await userService.GetUserAsync(userId);

        if (user is null)
        {
            return RemoveRoomRightsResponse.USER_NOT_EXISTS;
        }
        
        if (userId == room.OwnerId)
        {
            return RemoveRoomRightsResponse.USER_IS_OWNER;
        }

        RoomRight? roomRightAlreadyGiven = room.RoomRights
            .FirstOrDefault(r => r.UserId == userId);
        
        if (roomRightAlreadyGiven is null)
        {
            return RemoveRoomRightsResponse.USER_HAS_NOT_RIGHTS;
        }

        bool success = await roomRuntimeService
            .RemoveRoomRightsAsync(roomId, roomRightAlreadyGiven);

        if (!success)
        {
            return RemoveRoomRightsResponse.FAIL;
        }
        
        gameDbContext
            .Remove(roomRightAlreadyGiven);
        
        await gameDbContext.SaveChangesAsync();
        
        return RemoveRoomRightsResponse.SUCCESS;   
    }


    public async Task<BanUserFromRoomResponse> BanUserAsync(string roomId, string actorId, string userId)
    {
        // TODO: implement room rights to ban
        
        Room? room = await GetRoomAsync(roomId);

        if (room is null)
        {
            return BanUserFromRoomResponse.ROOM_NOT_EXISTS;
        }
        
        User? user = await userService.GetUserAsync(userId);

        if (user is null)
        {
            return BanUserFromRoomResponse.USER_NOT_EXISTS;
        }
        
        if (userId == room.OwnerId)
        {
            return BanUserFromRoomResponse.USER_IS_OWNER;
        }
        
        bool isUserAlreadyBanned = room.RoomBans
            .Any(b => b.UserId == userId);

        if (isUserAlreadyBanned)
        {
            return BanUserFromRoomResponse.ALREADY_BANNED;
        }

        RoomBan newBan = new()
        {
            UserId = userId,
            RoomId = room.Id,
        };

        bool success = await roomRuntimeService.BanUserAsync(
            roomId,
            newBan);

        if (!success)
        {
            return BanUserFromRoomResponse.FAIL;
        }

        gameDbContext.RoomBans
            .Add(newBan);
        
        await gameDbContext.SaveChangesAsync();

        return BanUserFromRoomResponse.SUCCESS;
    }


    public async Task<UnbanUserFromRoomResponse> UnbanUserAsync(string roomId, string actorId, string userId)
    {
        Room? room = await GetRoomAsync(roomId);

        if (room is null)
        {
            return UnbanUserFromRoomResponse.ROOM_NOT_EXISTS;
        }
        
        if (room.OwnerId != actorId)
        {
            return UnbanUserFromRoomResponse.CANNOT_UNBAN;
        }
        
        User? user = await userService.GetUserAsync(userId);

        if (user is null)
        {
            return UnbanUserFromRoomResponse.USER_NOT_EXISTS;
        }

        RoomBan? roomBan = room.RoomBans
            .FirstOrDefault(b => b.UserId == userId);

        if (roomBan is null)
        {
            return UnbanUserFromRoomResponse.USER_NOT_BANNED;
        }

        bool success = await roomRuntimeService
            .UnbanUserAsync(roomId, roomBan);

        if (!success)
        {
            return UnbanUserFromRoomResponse.FAIL;
        }
        
        gameDbContext.RoomBans
            .Remove(roomBan);
        
        await gameDbContext.SaveChangesAsync();
        
        return UnbanUserFromRoomResponse.SUCCESS;
    }


    public async Task<UnbanAllFromRoomResponse> UnbanAllAsync(string roomId, string actorId)
    {
        Room? room = await GetRoomAsync(roomId);

        if (room is null)
        {
            return UnbanAllFromRoomResponse.ROOM_NOT_EXISTS;
        }
        
        if (room.OwnerId != actorId)
        {
            return UnbanAllFromRoomResponse.CANNOT_UNBAN;
        }
        
        List<RoomBan> bannedPlayers = room.RoomBans
            .ToList();
        
        bool isThereBannedPlayers = bannedPlayers
            .Any(b => b.UserId == actorId);

        if (!isThereBannedPlayers)
        {
            return UnbanAllFromRoomResponse.NONE_TO_UNBAN;
        }
        
        bool success = await roomRuntimeService.UnbanAllAsync(roomId);

        if (!success)
        {
            return UnbanAllFromRoomResponse.FAIL;
        }
        
        gameDbContext.RoomBans
            .RemoveRange(bannedPlayers);

        await gameDbContext.SaveChangesAsync();
        
        return UnbanAllFromRoomResponse.SUCCESS;
    }

    
    public async Task<AddBannedWordResponse> AddBannedWordAsync(string roomId, string actorId, string word)
    {
        if (word.Length is < 1 or > 75)
        {
            return AddBannedWordResponse.WORD_LENGTH;
        }
        
        Room? room = await GetRoomAsync(roomId);

        if (room is null)
        {
            return AddBannedWordResponse.ROOM_NOT_EXISTS;
        }

        if (room.OwnerId != actorId)
        {
            return AddBannedWordResponse.MUST_BE_OWNER;
        }
        
        bool isWordAlreadyBanned = room.RoomBannedWords
            .Any(b => b.BannedWord == word);

        if (isWordAlreadyBanned)
        {
            return AddBannedWordResponse.WORD_ALREADY_BANNED;
        }

        RoomBannedWord bannedWord = new()
        {
            RoomId = room.Id,
            BannedWord = word,
        };

        bool success = await roomRuntimeService
            .BanWord(roomId, bannedWord);

        if (!success)
        {
            return AddBannedWordResponse.FAIL;
        }
        
        gameDbContext.RoomBannedWords
            .Add(bannedWord);
        
        await gameDbContext.SaveChangesAsync();
        
        return AddBannedWordResponse.SUCCESS;
    }

    
    public async Task<RemoveBannedWordResponse> RemoveBannedWordAsync(string roomId, string actorId, string word)
    {
        if (word.Length is < 1 or > 75)
        {
            return RemoveBannedWordResponse.WORD_LENGTH;
        }
        
        Room? room = await GetRoomAsync(roomId);

        if (room is null)
        {
            return RemoveBannedWordResponse.ROOM_NOT_EXISTS;
        }

        if (room.OwnerId != actorId)
        {
            return RemoveBannedWordResponse.MUST_BE_OWNER;
        }
        
        RoomBannedWord? bannedWord = room.RoomBannedWords
            .FirstOrDefault(b => b.BannedWord == word);

        if (bannedWord is null)
        {
            return RemoveBannedWordResponse.WORD_NOT_BANNED;
        }
        
        bool success = await roomRuntimeService
            .UnbanWord(roomId, bannedWord);

        if (!success)
        {
            return RemoveBannedWordResponse.FAIL;
        }
        
        gameDbContext.RoomBannedWords
            .Remove(bannedWord);
        
        await gameDbContext.SaveChangesAsync();
        
        return RemoveBannedWordResponse.SUCCESS;
    }
}