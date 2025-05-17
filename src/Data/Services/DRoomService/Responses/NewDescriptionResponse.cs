namespace Iso.Data.Services.DRoomService.Responses;

public enum NewDescriptionResponse
{
    SUCCESS = ServiceResponse.SUCCESS,
    FAIL = ServiceResponse.FAIL,
    
    ROOM_NOT_EXISTS = RoomServiceResponse.ROOM_NOT_EXISTS,
    
    DESCRIPTION_LENGTH = 100,
    MUST_BE_OWNER = 101,
}