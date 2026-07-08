using Microsoft.AspNetCore.SignalR;

namespace TrainJourneyWebApis.Hubs
{
    public class JourneyHub : Hub
    {
        // Clients will connect to this hub to receive real-time updates.
        // We don't need incoming methods here yet, as the API will broadcast directly.
    }
}
