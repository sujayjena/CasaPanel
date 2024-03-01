using CasaAPI.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;

namespace CasaAPI.Services
{
    public class MessageHubClient : Hub<IMessageHubClient>
    {
        public async Task SendVisitNotificationToEmployee(List<string> message)
        {
            await Clients.All.SendVisitNotificationToEmployee(message);
        }
    }
}
