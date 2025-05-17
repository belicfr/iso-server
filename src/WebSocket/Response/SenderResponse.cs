namespace Iso.WebSocket.Response;

public record SenderResponse(
    SenderResponseCode Code,
    string? Message = null,
    List<object>? Props = null);