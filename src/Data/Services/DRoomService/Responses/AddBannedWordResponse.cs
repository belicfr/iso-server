namespace Iso.Data.Services.DRoomService.Responses;

public enum AddBannedWordResponse
{
    SUCCESS = ServiceResponse.SUCCESS,
    FAIL = ServiceResponse.FAIL,
    
    ROOM_NOT_EXISTS = RoomServiceResponse.ROOM_NOT_EXISTS,
    
    MUST_BE_OWNER = 101,
    WORD_ALREADY_BANNED = 102,
    WORD_LENGTH = 103,
}