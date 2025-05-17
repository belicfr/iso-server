namespace Iso.Data.Services.DRoomService.Responses;

public enum NewNameResponse
{
    SUCCESS = ServiceResponse.SUCCESS,
    FAIL = ServiceResponse.FAIL,
    
    ROOM_NOT_EXISTS = RoomServiceResponse.ROOM_NOT_EXISTS,
    
    NAME_LENGTH = 100,
    MUST_BE_OWNER = 101,
}