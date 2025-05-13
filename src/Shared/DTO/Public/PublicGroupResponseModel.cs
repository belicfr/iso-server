using Iso.Data.Models.RoomModel;

namespace Iso.Shared.DTO.Public;

public record PublicGroupResponseModel(
    string id,
    string name,
    string description,
    string ownerId,
    GroupMode mode,
    string roomId,
    DateTime createdAt);