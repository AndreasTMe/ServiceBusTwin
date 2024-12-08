using System.Threading.Tasks;
using Xunit;

namespace ServiceBusTwin.Tests;

public class ServiceBusTests : IAsyncLifetime
{
    // Recreating the example provided by Microsoft (see file 'config.jsonc')
    private readonly IEmulator _emulator = new EmulatorBuilder()
        .WithQueue("queue.1", options => options.MaxDeliveryCount = 10)
        .WithTopic(
            "topic.1",
            topicOptions =>
            {
                topicOptions
                    .AddSubscription(
                        "subscription.1",
                        subscriptionOptions =>
                        {
                            subscriptionOptions.MaxDeliveryCount = 10;
                            subscriptionOptions.AddRule(
                                "app-prop-filter-1",
                                ruleOptions =>
                                {
                                    ruleOptions.FilterType = RuleFilterType.Correlation;
                                    ruleOptions
                                        .AddSystemPropertyFilter(SystemProperties.ContentType, "application/text")
                                        .AddSystemPropertyFilter(SystemProperties.CorrelationId, "id1")
                                        .AddSystemPropertyFilter(SystemProperties.Label, "subject1")
                                        .AddSystemPropertyFilter(SystemProperties.MessageId, "msgid1")
                                        .AddSystemPropertyFilter(SystemProperties.ReplyTo, "someQueue")
                                        .AddSystemPropertyFilter(SystemProperties.ReplyToSessionId, "sessionId")
                                        .AddSystemPropertyFilter(SystemProperties.SessionId, "session1")
                                        .AddSystemPropertyFilter(SystemProperties.To, "xyz");
                                });
                        })
                    .AddSubscription(
                        "subscription.2",
                        subscriptionOptions =>
                        {
                            subscriptionOptions.MaxDeliveryCount = 10;
                            subscriptionOptions.AddRule(
                                "user-prop-filter-1",
                                ruleOptions =>
                                {
                                    ruleOptions.FilterType = RuleFilterType.Correlation;
                                    ruleOptions.AddUserPropertyFilter("prop3", "value3");
                                });
                        })
                    .AddSubscription(
                        "subscription.3",
                        subscriptionOptions => subscriptionOptions.MaxDeliveryCount = 10);
            })
        .Build();

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