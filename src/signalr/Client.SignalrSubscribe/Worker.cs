using Common;
using Microsoft.AspNetCore.SignalR.Client;

namespace Client.SignalrSubscribe;

public class Worker : IHostedService
{
    private readonly ILogger<Worker> _logger;
    private readonly HubConnection _connection;

    public Worker(ILogger<Worker> logger, HubConnection connection)
    {
        _logger = logger;
        _connection = connection;
        _connection.On<string,string>(SignalrMethods.TimeBroadcast, (connectionId, timeString) =>
        {
            _logger.LogInformation("\nMethod: {method}", SignalrMethods.TimeBroadcast);
            _logger.LogInformation("First argument (Connection ID): {connectionId}", connectionId);
            _logger.LogInformation("Second argument (Time String): {timeString}", timeString);
        });
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _connection.StartAsync(cancellationToken);
        _logger.LogInformation("Connection established! ID: {id}", _connection.ConnectionId);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Connection aborted! ID: {id}", _connection.ConnectionId);
        await _connection.StopAsync(cancellationToken);
    }
}