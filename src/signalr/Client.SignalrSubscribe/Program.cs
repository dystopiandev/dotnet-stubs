using Client.SignalrSubscribe;
using Microsoft.AspNetCore.SignalR.Client;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton(new HubConnectionBuilder()
            .WithUrl("http://localhost:55335/messages")
            .Build());
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();