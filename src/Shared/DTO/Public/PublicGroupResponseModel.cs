namespace Iso.Shared.DTO.Public;

public record PublicGroupResponseModel(
    string id,
    string name,
    string description,
    string ownerId,
    int mode,
    string roomId,
    DateTime createdAt);