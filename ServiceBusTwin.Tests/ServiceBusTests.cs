using System.Threading.Tasks;
using Xunit;

namespace ServiceBusTwin.Tests;

public class ServiceBusTests : IAsyncLifetime
{
    private readonly IEmulator _emulator = new EmulatorBuilder().Build();

    public Task InitializeAsync() => _emulator.StartAsync();

    public async Task DisposeAsync()
    {
        await _emulator.StopAsync();
        await _emulator.DisposeAsync();
    }

    [Fact]
    public void DoTheThing()
    {
    }
}