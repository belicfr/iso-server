using Iso.Shared.Physic;

namespace Iso.Shared.DTO.Public;

public record PublicAccountResponseModel(
    string Id,
    string UserName,
    string NormalizedUserName,
    Coord2D TileCoord);