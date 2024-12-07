namespace ServiceBusTwin;

public interface IEmulator : IAsyncDisposable
{
    Task StartAsync();

    Task StopAsync();
}