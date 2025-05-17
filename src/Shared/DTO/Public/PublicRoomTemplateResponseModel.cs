namespace Iso.Shared.DTO.Public;

public record PublicRoomTemplateResponseModel(
    string id,
    string name,
    string template,
    int tilesCount);