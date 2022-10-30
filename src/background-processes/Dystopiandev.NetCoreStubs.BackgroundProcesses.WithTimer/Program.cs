using Dystopiandev.NetCoreStubs.BackgroundProcesses.WithTimer;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => { services.AddHostedService<Worker>(); })
    .Build();

await host.RunAsync();