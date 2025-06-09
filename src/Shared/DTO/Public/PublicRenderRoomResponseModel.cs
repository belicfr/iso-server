namespace Iso.Shared.DTO.Public;

public record PublicRenderRoomResponseModel(
    string id,
    string name,
    string description,
    int playersInRoomCount,
    HashSet<PublicAccountResponseModel> playersInRoom,
    int playersLimit,
    string template,
    PublicGroupResponseModel? group,
    string ownerId,
    string? tagOne,
    string? tagTwo,
    bool isPublic,
    string thumbnail);