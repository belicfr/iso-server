using Iso.Shared.DTO.Public;
using Microsoft.AspNetCore.SignalR;

namespace Iso.WebSocket.Hubs;

public partial class GameHub
{
    public async Task SendRoomTemplates()
    {
        const string responseChannel = "ReceiveRoomTemplates";

        IEnumerable<PublicRoomTemplateResponseModel> templates
            = (await roomTemplateService.GetAllRoomTemplatesAsync())
                .Select(t => new PublicRoomTemplateResponseModel(
                    t.Id,
                    t.Name,
                    t.Template,
                    t.TilesCount));
        
        await Clients.Caller.SendAsync(
            responseChannel,
            templates);
    }
}