namespace Iso.Shared.DTO.Restricted;

public record RestrictedAccountResponseModel(
    string id,
    string name,
    string normalizedName,
    string? homeRoomId,
    int crowns,
    List<string> friends);