using Iso.Shared.Physic;

namespace Iso.Shared.DTO.Restricted;

public record RestrictedAccountResponseModel(
    string Id,
    string Name,
    string NormalizedName,
    string? HomeRoomId,
    int Crowns,
    List<string> Friends,
    Coord2D Position);