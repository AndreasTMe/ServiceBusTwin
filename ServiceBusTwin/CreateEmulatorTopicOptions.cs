namespace ServiceBusTwin;

public interface ITopicWithSubscription
{
    ITopicWithSubscription WithSubscription(string name,
                                            Action<CreateEmulatorSubscriptionOptions>? configure = default);
}

public sealed class CreateEmulatorTopicOptions : ITopicWithSubscription
{
    public TimeSpan DefaultMessageTimeToLive { get; set; } = TimeSpan.FromHours(1);

    public TimeSpan DuplicateDetectionHistoryTimeWindow { get; set; } = TimeSpan.FromSeconds(20);

    public bool RequiresDuplicateDetection { get; set; }

    internal Dictionary<string, CreateEmulatorSubscriptionOptions> SubscriptionOptions { get; } = [];

    public ITopicWithSubscription WithSubscription(string name,
                                                   Action<CreateEmulatorSubscriptionOptions>? configure = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        var options = new CreateEmulatorSubscriptionOptions();
        configure?.Invoke(options);

        SubscriptionOptions.TryAdd(name, options);
        return this;
    }
}