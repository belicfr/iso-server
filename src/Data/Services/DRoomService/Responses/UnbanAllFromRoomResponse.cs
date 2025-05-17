namespace Iso.Data.Services.DRoomService.Responses;

public enum UnbanAllFromRoomResponse
{
    SUCCESS = ServiceResponse.SUCCESS,
    FAIL = ServiceResponse.FAIL,
    
    ROOM_NOT_EXISTS = RoomServiceResponse.ROOM_NOT_EXISTS,
    
    NONE_TO_UNBAN = 100,
    CANNOT_UNBAN = 101,
}