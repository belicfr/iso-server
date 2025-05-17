using Iso.Data.Models.CreationModels;
using Iso.Data.Models.RoomModel;
using Iso.Data.Models.UserModel;
using Iso.Data.Services.DRoomService.Responses;
using Iso.Shared.DTO.Public;
using Iso.WebSocket.Response;
using Microsoft.AspNetCore.SignalR;

namespace Iso.WebSocket.Hubs;

public partial class GameHub
{
    public async Task SendAllRooms()
    {
        const string responseChannel = "ReceiveAllRooms";

        IEnumerable<PublicNavigatorRoomResponseModel> rooms
            = PrepareRooms(await roomService.GetAllRoomsAsync());
        
        await Clients.Caller.SendAsync(responseChannel, rooms);
    }

    public async Task SendPublicRooms()
    {
        const string responseChannel = "ReceivePublicRooms";
        
        IEnumerable<PublicNavigatorRoomResponseModel> rooms
            = PrepareRooms(await roomService.GetPublicRoomsAsync());
        
        await Clients.Caller.SendAsync(responseChannel, rooms);
    }

    public async Task SendPlayerRooms()
    {
        const string responseChannel = "ReceivePlayerRooms";
        
        User? user = await GetUserBySso();

        if (user is null)
        {
            await Clients.Caller.SendAsync(
                responseChannel, 
                Array.Empty<PublicNavigatorRoomResponseModel>());

            return;
        }

        IEnumerable<PublicNavigatorRoomResponseModel> rooms
            = PrepareRooms(
                await userService.GetRoomsForUserAsync(user.Id));
        
        await Clients.Caller.SendAsync(responseChannel, rooms);
    }

    public async Task SendRoom(string roomId)
    {
        const string responseChannel = "ReceiveRoom";

        User? user = await GetUserBySso();
        
        if (user is null)
        {
            await Clients.Caller.SendAsync(
                responseChannel, 
                null);

            return;
        }
        
        Room? room = await roomService.GetRoomAsync(roomId);

        if (room is null)
        {
            await Clients.Caller.SendAsync(
                responseChannel,
                null);

            return;
        }

        object roomResponseModel;

        if (room.OwnerId == user.Id)
        {
            roomResponseModel 
                = await PrepareRoomForOwner(room);
        }
        else
        {
            roomResponseModel 
                = PrepareRoom(room);
        }

        await Clients.Caller.SendAsync(responseChannel, roomResponseModel);
    }

    public async Task SendNewRoomName(string roomId, string name)
    {
        const string responseChannel = "ReceiveNewRoomName";

        string? actorId = (await GetUserBySso())?
            .Id;
        
        NewNameResponse response = actorId is null
            ? NewNameResponse.FAIL
            : await roomService.SaveNewNameAsync(roomId, actorId, name);

        SenderResponseCode code = response == NewNameResponse.SUCCESS
            ? SenderResponseCode.SUCCESS
            : SenderResponseCode.FAIL;

        string? message;

        switch (response)
        {
            case NewNameResponse.SUCCESS:
                message = null;
                break;

            case NewNameResponse.ROOM_NOT_EXISTS:
                message = "The room doesn't exist.";
                break;
                
            case NewNameResponse.NAME_LENGTH:
                message = "Name must be between 1 and 50 characters long.";
                break;
            
            default:
                message = "An error occured. (Server Error)";
                break;
        }

        await Clients.Caller.SendAsync(
            responseChannel,
            new SenderResponse(
                code,
                message));
    }
    
    public async Task SendNewRoomDescription(string roomId, string description)
    {
        const string responseChannel = "ReceiveNewRoomDescription";
        
        string? actorId = (await GetUserBySso())?
            .Id;

        NewDescriptionResponse response = actorId is null
            ? NewDescriptionResponse.FAIL
            : await roomService.SaveNewDescriptionAsync(roomId, actorId, description);
        
        SenderResponseCode code = response == NewDescriptionResponse.SUCCESS
            ? SenderResponseCode.SUCCESS
            : SenderResponseCode.FAIL;

        string? message;

        switch (response)
        {
            case NewDescriptionResponse.SUCCESS:
                message = null;
                break;

            case NewDescriptionResponse.ROOM_NOT_EXISTS:
                message = "The room doesn't exist.";
                break;
                
            case NewDescriptionResponse.DESCRIPTION_LENGTH:
                message = "Description must be between 1 and 1024 characters long.";
                break;
            
            default:
                message = "An error occured. (Server Error)";
                break;
        }

        await Clients.Caller.SendAsync(
            responseChannel,
            new SenderResponse(
                code,
                message));
    }
    
    public async Task SendNewRoomTag(string roomId, int position, string tag)
    {
        const string responseChannel = "ReceiveNewRoomDescription";
        
        string? actorId = (await GetUserBySso())?
            .Id;

        NewTagResponse response = actorId is null
            ? NewTagResponse.FAIL
            : await roomService.SaveNewTagAsync(roomId, actorId, position, tag);
        
        SenderResponseCode code = response == NewTagResponse.SUCCESS
            ? SenderResponseCode.SUCCESS
            : SenderResponseCode.FAIL;

        string? message;

        switch (response)
        {
            case NewTagResponse.SUCCESS:
                message = null;
                break;

            case NewTagResponse.ROOM_NOT_EXISTS:
                message = "The room doesn't exist.";
                break;
            
            case NewTagResponse.INVALID_POSITION:
                message = "Invalid tag position.";
                break;
                
            case NewTagResponse.TAG_LENGTH:
                message = "Tag must be between 1 and 10 characters long.";
                break;
            
            default:
                message = "An error occured. (Server Error)";
                break;
        }

        await Clients.Caller.SendAsync(
            responseChannel,
            new SenderResponse(
                code,
                message));
    }

    public async Task SendNewRoomRight(string roomId, string userId)
    {
        const string responseChannel = "ReceiveNewRoomRight";

        string? actorId = (await GetUserBySso())?
            .Id;
        
        User? user = await userService.GetUserAsync(userId);
        Room? room = await roomService.GetRoomAsync(roomId);
        
        AddRoomRightsResponse response = AddRoomRightsResponse.FAIL;
        SenderResponseCode code = SenderResponseCode.FAIL;

        PublicAccountResponseModel userResponseModel;
        List<object>? props = null;

        if (actorId is not null && user is not null && room is not null)
        {
            response = await roomService.AddRoomRightsAsync(roomId, actorId, userId);
        
            code = response == AddRoomRightsResponse.SUCCESS
                ? SenderResponseCode.SUCCESS
                : SenderResponseCode.FAIL;

            userResponseModel = new PublicAccountResponseModel(
                user.Id,
                user.UserName ?? "Unknown",
                user.NormalizedUserName ?? "UNKNOWN");
            
            props = [userResponseModel];
        }
        else if (actorId is null)
        {
            response = AddRoomRightsResponse.FAIL;
        }
        else if (user is null)
        {
            response = AddRoomRightsResponse.USER_NOT_EXISTS;
        }
        else if (room is null)
        {
            response = AddRoomRightsResponse.ROOM_NOT_EXISTS;
        }

        string? message;

        switch (response)
        {
            case AddRoomRightsResponse.SUCCESS:
                message = null;
                break;

            case AddRoomRightsResponse.ROOM_NOT_EXISTS:
                message = "The room doesn't exist.";
                break;
            
            case AddRoomRightsResponse.USER_NOT_EXISTS:
                message = "The user doesn't exist.";
                break;
            
            case AddRoomRightsResponse.ALREADY_HAS_RIGHTS:
                message = "This player already has rights.";
                break;
            
            case AddRoomRightsResponse.USER_IS_OWNER:
                message = "The owner already has rights.";
                break;
            
            default:
                message = "An error occured. (Server Error)";
                break;
        }

        await Clients.Caller.SendAsync(
            responseChannel,
            new SenderResponse(
                code,
                message,
                props));
    }

    public async Task SendRemoveRoomRight(string roomId, string userId)
    {
        const string responseChannel = "ReceiveRemoveRoomRight";

        string? actorId = (await GetUserBySso())?
            .Id;
        
        User? user = await userService.GetUserAsync(userId);
        Room? room = await roomService.GetRoomAsync(roomId);

        RemoveRoomRightsResponse response = RemoveRoomRightsResponse.FAIL;
        SenderResponseCode code = SenderResponseCode.FAIL;
        
        PublicAccountResponseModel userResponseModel;
        List<object>? props = null;
        
        if (actorId is not null && user is not null && room is not null)
        {
            response = await roomService.RemoveRoomRightsAsync(roomId, actorId, userId);
        
            code = response == RemoveRoomRightsResponse.SUCCESS
                ? SenderResponseCode.SUCCESS
                : SenderResponseCode.FAIL;

            userResponseModel = new PublicAccountResponseModel(
                user.Id,
                user.UserName ?? "Unknown",
                user.NormalizedUserName ?? "UNKNOWN");
            
            props = [userResponseModel];
        }
        else if (actorId is null)
        {
            response = RemoveRoomRightsResponse.FAIL;
        }
        else if (user is null)
        {
            response = RemoveRoomRightsResponse.USER_NOT_EXISTS;
        }
        else if (room is null)
        {
            response = RemoveRoomRightsResponse.ROOM_NOT_EXISTS;
        }

        string? message;

        switch (response)
        {
            case RemoveRoomRightsResponse.SUCCESS:
                message = null;
                break;

            case RemoveRoomRightsResponse.ROOM_NOT_EXISTS:
                message = "The room doesn't exist.";
                break;
            
            case RemoveRoomRightsResponse.USER_NOT_EXISTS:
                message = "The user doesn't exist.";
                break;
            
            case RemoveRoomRightsResponse.USER_HAS_NOT_RIGHTS:
                message = "This player does not have rights.";
                break;
            
            case RemoveRoomRightsResponse.USER_IS_OWNER:
                message = "The owner must have rights.";
                break;
            
            default:
                message = "An error occured. (Server Error)";
                break;
        }

        await Clients.Caller.SendAsync(
            responseChannel,
            new SenderResponse(
                code,
                message,
                props));
    }

    public async Task SendBanUserFromRoom(string roomId, string userId)
    {
        const string responseChannel = "ReceiveBanUserFromRoom";
        
        string? actorId = (await GetUserBySso())?
            .Id;

        BanUserFromRoomResponse response = actorId is not null
            ? await roomService.BanUserAsync(roomId, actorId, userId)
            : BanUserFromRoomResponse.FAIL;
        
        SenderResponseCode code = response == BanUserFromRoomResponse.SUCCESS
            ? SenderResponseCode.SUCCESS
            : SenderResponseCode.FAIL;

        string? message;

        switch (response)
        {
            case BanUserFromRoomResponse.SUCCESS:
                message = null;
                break;

            case BanUserFromRoomResponse.ROOM_NOT_EXISTS:
                message = "The room doesn't exist.";
                break;
            
            case BanUserFromRoomResponse.USER_NOT_EXISTS:
                message = "The user doesn't exist.";
                break;
            
            case BanUserFromRoomResponse.USER_IS_OWNER:
                message = "The owner must have rights.";
                break;
            
            case BanUserFromRoomResponse.ALREADY_BANNED:
                message = "This player is already banned.";
                break;
            
            default:
                message = "An error occured. (Server Error)";
                break;
        }

        await Clients.Caller.SendAsync(
            responseChannel,
            new SenderResponse(
                code,
                message));
    }

    public async Task SendUnbanUserFromRoom(string roomId, string userId)
    {
        const string responseChannel = "ReceiveUnbanUserFromRoom";
        
        User? user = await userService.GetUserAsync(userId);

        UnbanUserFromRoomResponse response;
        SenderResponseCode code = SenderResponseCode.FAIL;
        
        List<object>? props = null;
        PublicAccountResponseModel userResponseModel;

        string? actorId = (await GetUserBySso())?
            .Id;
        
        if (user is not null && actorId is not null)
        {
            response = await roomService.UnbanUserAsync(
                roomId,
                actorId,
                userId);
        
            code = response == UnbanUserFromRoomResponse.SUCCESS
                ? SenderResponseCode.SUCCESS
                : SenderResponseCode.FAIL;

            userResponseModel = new PublicAccountResponseModel(
                user.Id,
                user.UserName ?? "Unknown",
                user.NormalizedUserName ?? "UNKNOWN");
            
            props = [userResponseModel];
        }
        else
        {
            response = UnbanUserFromRoomResponse.USER_NOT_EXISTS;
        }

        string? message;

        switch (response)
        {
            case UnbanUserFromRoomResponse.SUCCESS:
                message = null;
                break;

            case UnbanUserFromRoomResponse.ROOM_NOT_EXISTS:
                message = "The room doesn't exist.";
                break;
            
            case UnbanUserFromRoomResponse.USER_NOT_EXISTS:
                message = "The user doesn't exist.";
                break;
            
            case UnbanUserFromRoomResponse.USER_NOT_BANNED:
                message = "This player is not banned.";
                break;
            
            default:
                message = "An error occured. (Server Error)";
                break;
        }

        await Clients.Caller.SendAsync(
            responseChannel,
            new SenderResponse(
                code,
                message,
                props));
    }

    public async Task SendUnbanAllFromRoom(string roomId)
    {
        const string responseChannel = "ReceiveUnbanAllFromRoom";
        
        string? actorId = (await GetUserBySso())?
            .Id; 
        
        UnbanAllFromRoomResponse response = UnbanAllFromRoomResponse.FAIL;
        SenderResponseCode code = SenderResponseCode.FAIL;

        if (actorId is not null)
        {
            response = await roomService.UnbanAllAsync(roomId, actorId);
            
            code = response == UnbanAllFromRoomResponse.SUCCESS
                ? SenderResponseCode.SUCCESS
                : SenderResponseCode.FAIL;
        }
        
        string? message;

        switch (response)
        {
            case UnbanAllFromRoomResponse.SUCCESS:
                message = null;
                break;
            
            case UnbanAllFromRoomResponse.ROOM_NOT_EXISTS:
                message = "The room does not exist.";
                break;
            
            case UnbanAllFromRoomResponse.CANNOT_UNBAN:
                message = "You must be owner to unban here.";
                break;
            
            case UnbanAllFromRoomResponse.NONE_TO_UNBAN:
                message = "No one player is banned here.";
                break;
            
            default:
                message = "An error occured. (Server Error)";
                break;
        }

        await Clients.Caller.SendAsync(
            responseChannel,
            new SenderResponse(
                code,
                message));
    }

    public async Task SendAddBannedWord(string roomId, string word)
    {
        const string responseChannel = "ReceiveAddBannedWord";

        string? actorId = (await GetUserBySso())?
            .Id;
        
        AddBannedWordResponse response = AddBannedWordResponse.FAIL;
        SenderResponseCode code = SenderResponseCode.FAIL;

        if (actorId is not null)
        {
            response = await roomService
                .AddBannedWordAsync(roomId, actorId, word);
            
            code = response == AddBannedWordResponse.SUCCESS
                ? SenderResponseCode.SUCCESS
                : SenderResponseCode.FAIL;
        }

        string? message;

        switch (response)
        {
            case AddBannedWordResponse.SUCCESS:
                message = null;
                break;
            
            case AddBannedWordResponse.WORD_LENGTH:
                message = "The word must be between 1 and 75 characters.";
                break;
            
            case AddBannedWordResponse.MUST_BE_OWNER:
                message = "You do not have permission to add banned words.";
                break;
            
            case AddBannedWordResponse.ROOM_NOT_EXISTS:
                message = "The room does not exist.";
                break;
            
            case AddBannedWordResponse.WORD_ALREADY_BANNED:
                message = "This word is already banned.";
                break;
            
            default: 
                message = "An error occured. (Server Error)";
                break;
        }

        await Clients.Caller.SendAsync(
            responseChannel,
            new SenderResponse(
                code,
                message,
                [word]));
    }
    
    public async Task SendRemoveBannedWord(string roomId, string word)
    {
        const string responseChannel = "ReceiveRemoveBannedWord";

        string? actorId = (await GetUserBySso())?
            .Id;
        
        RemoveBannedWordResponse response = RemoveBannedWordResponse.FAIL;
        SenderResponseCode code = SenderResponseCode.FAIL;

        if (actorId is not null)
        {
            response = await roomService
                .RemoveBannedWordAsync(roomId, actorId, word);
            
            code = response == RemoveBannedWordResponse.SUCCESS
                ? SenderResponseCode.SUCCESS
                : SenderResponseCode.FAIL;
        }

        string? message;

        switch (response)
        {
            case RemoveBannedWordResponse.SUCCESS:
                message = null;
                break;
            
            case RemoveBannedWordResponse.WORD_LENGTH:
                message = "The word must be between 1 and 75 characters.";
                break;
            
            case RemoveBannedWordResponse.MUST_BE_OWNER:
                message = "You do not have permission to add banned words.";
                break;
            
            case RemoveBannedWordResponse.ROOM_NOT_EXISTS:
                message = "The room does not exist.";
                break;
            
            case RemoveBannedWordResponse.WORD_NOT_BANNED:
                message = "This word is not banned.";
                break;
            
            default: 
                message = "An error occured. (Server Error)";
                break;
        }

        await Clients.Caller.SendAsync(
            responseChannel,
            new SenderResponse(
                code,
                message,
                [word]));
    }

    public async Task SendCreateRoom(NewRoomCreationModel creationModel)
    {
        const string responseChannel = "ReceiveCreateRoom";

        string? actorId = (await GetUserBySso())?
            .Id;

        SenderResponseCode code = SenderResponseCode.FAIL;
        
        string? message = null;
        List<object>? props = null;
        Room? createdRoom = null;

        if (actorId is not null)
        {
            createdRoom = await roomService
                .CreateAsync(creationModel, actorId);
        }
        
        if (actorId is null || createdRoom is null)
        {
            message = "An error occured. The room is not created. (Server Error)";
        }
        else
        {
            code = SenderResponseCode.SUCCESS;
            props = [createdRoom];
        }

        await Clients.Caller.SendAsync(
            responseChannel,
            new SenderResponse(
                code,
                message,
                props));
    }
}