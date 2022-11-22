using Common;
using Microsoft.AspNetCore.SignalR;

namespace Server;

public class MessageHub : Hub
{
    public async Task TimeBroadcast(string connectionId, string timeString)
    {
        await Clients.All.SendAsync(SignalrMethods.TimeBroadcast, connectionId, timeString);
    }
}