namespace Iso.Shared.DTO.Public;

public record PublicNavigatorRoomResponseModel(
    string id,
    string name,
    string description,
    int playersInRoomCount,
    int playersLimit,
    string template,
    PublicGroupResponseModel? group,
    string ownerId,
    List<string> tags,
    bool isPublic);