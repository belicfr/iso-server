using Iso.Shared.DTO.Public;
using Iso.Shared.Physic;

namespace Iso.WebSocket.Response.PlayerMove;

public record PlayerMoveResponse(
    PublicAccountResponseModel? Actor,
    List<Coord2D>? path);