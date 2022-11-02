namespace BackgroundProcesses.WithTimer;

public class Worker : BackgroundService, IDisposable
{
    public sealed override void Dispose()
    {
        _timer.Dispose();
        GC.SuppressFinalize(this);
    }

    private readonly ILogger<Worker> _logger;
    private readonly PeriodicTimer _timer;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(2));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            
            while (await _timer.WaitForNextTickAsync(stoppingToken))
            {
                OnTick();
            }
        }
    }

    private void OnTick()
    {
        _logger.LogInformation("Timer ticked at {Now}", DateTime.Now);
    }
}