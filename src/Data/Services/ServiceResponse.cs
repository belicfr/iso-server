namespace Iso.Data.Services;

public record ServiceResponse(
    ServiceResponseCode Code,
    string? Message = null);