using RabbitmqProducer;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => { services.AddHostedService<AmqpProducer>(); })
    .Build();

await host.RunAsync();