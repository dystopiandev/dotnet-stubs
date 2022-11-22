using Common;
using Microsoft.AspNetCore.SignalR.Client;

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:55335/messages")
    .Build();

await connection.StartAsync();
Console.WriteLine($"Connection established! ID: {connection.ConnectionId}");

var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
var count = 1;

while (await timer.WaitForNextTickAsync() && count <= 10)
{
    Console.WriteLine($"Sending broadcast {count}/10...");
    await connection.InvokeAsync(SignalrMethods.TimeBroadcast, connection.ConnectionId, DateTime.Now.ToString());
    count++;
}

await connection.StopAsync();