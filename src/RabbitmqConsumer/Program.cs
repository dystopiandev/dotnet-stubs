using RabbitmqConsumer;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => { services.AddHostedService<AmqpConsumer>(); })
    .Build();

await host.RunAsync();