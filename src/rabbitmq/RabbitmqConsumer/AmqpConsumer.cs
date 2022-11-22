using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitmqConsumer;

public class AmqpConsumer : BackgroundService
{
    private readonly ILogger<AmqpConsumer> _logger;
    private readonly IModel _consumerChannel;

    public AmqpConsumer(ILogger<AmqpConsumer> logger)
    {
        _logger = logger;
        _consumerChannel = new ConnectionFactory { Uri = new Uri("amqp://localhost:5672") }.CreateConnection().CreateModel();
        _consumerChannel.QueueDeclare("time-series", true, false, false);
        _consumerChannel.BasicQos(0, 1,false);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Consumer running at: {Time}", DateTimeOffset.Now);
            
            var consumer = new EventingBasicConsumer(_consumerChannel);

            consumer.Received += (sender, e) =>
            {
                var msg = Encoding.UTF8.GetString(e.Body.ToArray());
                
                _logger.LogInformation("Message received: {Msg}", msg);
                
                _consumerChannel.BasicAck(e.DeliveryTag, false);
            };
        }

        return Task.CompletedTask;
    }
}