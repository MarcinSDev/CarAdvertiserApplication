using Microsoft.AspNet.SignalR;

namespace CarAdvertiser.Hubs
{
    public class CarAdvertiserHub : Hub
    {
        public static void Show()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<CarAdvertiserHub>();
            context.Clients.All.displayMessage();
        }
    }
}