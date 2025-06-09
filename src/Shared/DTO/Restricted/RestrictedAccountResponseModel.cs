using Iso.Shared.Physic;

namespace Iso.Shared.DTO.Restricted;

public record RestrictedAccountResponseModel(
    string Id,
    string UserName,
    string NormalizedUserName,
    string? HomeRoomId,
    int Crowns,
    List<string> Friends,
    Coord2D TileCoord);