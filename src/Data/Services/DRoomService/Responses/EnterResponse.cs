namespace Iso.Data.Services.DRoomService.Responses;

public enum EnterResponse
{
    SUCCESS = ServiceResponse.SUCCESS,
    FAIL = ServiceResponse.FAIL,
    
    ROOM_NOT_EXISTS = RoomServiceResponse.ROOM_NOT_EXISTS,
    
    ROOM_FULL = 100,
    USER_BANNED = 101,
}