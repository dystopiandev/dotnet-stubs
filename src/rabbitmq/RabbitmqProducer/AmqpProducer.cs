using System.Globalization;
using System.Text;
using RabbitMQ.Client;

namespace RabbitmqProducer;

public class AmqpProducer : BackgroundService
{
    private readonly ILogger<AmqpProducer> _logger;
    private readonly PeriodicTimer _timer;
    private readonly IModel _producerChannel;

    public AmqpProducer(ILogger<AmqpProducer> logger)
    {
        _logger = logger;
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(2));
        _producerChannel = new ConnectionFactory { Uri = new Uri("amqp://localhost:5672") }.CreateConnection().CreateModel();
        _producerChannel.QueueDeclare("time-series", true, false, false);
        _producerChannel.BasicQos(0, 1,false);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("AmqpProducer running at: {Time}", DateTimeOffset.Now);
            
            while (await _timer.WaitForNextTickAsync(stoppingToken))
            {
                _producerChannel.BasicPublish("time-series", "", body: Encoding.UTF8.GetBytes(DateTime.Now.ToString(CultureInfo.InvariantCulture)));
            }
        }
    }
}