namespace Iso.Data.Services.DRoomService.Responses;

public enum BanUserFromRoomResponse
{
    SUCCESS = ServiceResponse.SUCCESS,
    FAIL = ServiceResponse.FAIL,
    
    ROOM_NOT_EXISTS = RoomServiceResponse.ROOM_NOT_EXISTS,
    
    USER_NOT_EXISTS = 100,
    USER_IS_OWNER = 101,
    ALREADY_BANNED = 102,
}