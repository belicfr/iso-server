using Iso.Shared.DTO.Public;

namespace Iso.Shared.DTO.Restricted;

public record RestrictedRoomResponseModel(
    string id,
    string name,
    string description,
    int playersInRoomCount,
    int playersLimit,
    string template,
    PublicGroupResponseModel? group,
    string ownerId,
    string? tagOne,
    string? tagTwo,
    bool isPublic,
    string thumbnail,
    List<PublicAccountResponseModel> rights,
    List<PublicAccountResponseModel> bans,
    List<string> bannedWords);