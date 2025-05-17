namespace Iso.Data.Services.DRoomService.Responses;

public enum NewTagResponse
{
    SUCCESS = ServiceResponse.SUCCESS,
    FAIL = ServiceResponse.FAIL,
    
    ROOM_NOT_EXISTS = RoomServiceResponse.ROOM_NOT_EXISTS,
    
    INVALID_POSITION = 100,
    TAG_LENGTH = 101,
    MUST_BE_OWNER = 102,
}