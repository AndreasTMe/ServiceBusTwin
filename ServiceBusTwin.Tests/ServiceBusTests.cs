using System.Threading.Tasks;
using Xunit;

namespace ServiceBusTwin.Tests;

public class ServiceBusTests : IAsyncLifetime
{
    // Recreating the example provided by Microsoft (see 'config.jsonc' file in the test project directory)
    private readonly IEmulator _emulator = new ServiceBusEmulatorBuilder()
        .WithQueue("queue.1", options => options.MaxDeliveryCount = 10)
        .WithTopic(
            "topic.1",
            topic =>
            {
                topic.WithSubscription(
                        "subscription.1",
                        subscription =>
                        {
                            subscription.MaxDeliveryCount = 10;
                            subscription.WithRule(
                                "app-prop-filter-1",
                                rule =>
                                {
                                    rule.FilterType = RuleFilterType.Correlation;
                                    rule
                                        .WithContentType("application/text")
                                        .WithCorrelationId("id1")
                                        .WithLabel("subject1")
                                        .WithMessageId("msgid1")
                                        .WithReplyTo("someQueue")
                                        .WithReplyToSessionId("sessionId")
                                        .WithSessionId("session1")
                                        .WithTo("xyz");
                                });
                        })
                    .WithSubscription(
                        "subscription.2",
                        subscription =>
                        {
                            subscription.MaxDeliveryCount = 10;
                            subscription.WithRule(
                                "user-prop-filter-1",
                                rule =>
                                {
                                    rule.FilterType = RuleFilterType.Correlation;
                                    rule.WithUserPropertyFilter("prop3", "value3");
                                });
                        })
                    .WithSubscription(
                        "subscription.3",
                        subscription => subscription.MaxDeliveryCount = 10);
            })
        .WithLogging(EmulatorLogging.Console)
        .Build();

    public Task InitializeAsync() => _emulator.StartAsync();

    public async Task DisposeAsync()
    {
        await _emulator.StopAsync();
        await _emulator.DisposeAsync();
    }

    [Fact]
    public async Task DoTheThing()
    {
        _ = await _emulator.ServiceBus.GetLogsAsync();
    }
}