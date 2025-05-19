namespace Iso.Shared.DTO.Public;

public record PublicPromotionResponseModel(
    string id,
    string title,
    string thumbnailPath,
    string action,
    int position);