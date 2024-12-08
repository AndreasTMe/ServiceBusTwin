namespace ServiceBusTwin;

public interface IEmulator : IAsyncDisposable
{
    IContainer ServiceBus { get; }

    string GetConnectionString();

    Task StartAsync();

    Task StopAsync();
}