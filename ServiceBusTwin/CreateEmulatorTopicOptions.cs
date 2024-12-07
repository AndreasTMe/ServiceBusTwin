namespace ServiceBusTwin;

public sealed class CreateEmulatorTopicOptions
{
    public TimeSpan DefaultMessageTimeToLive { get; set; } = TimeSpan.FromHours(1);

    public TimeSpan DuplicateDetectionHistoryTimeWindow { get; set; } = TimeSpan.FromSeconds(20);

    public bool RequiresDuplicateDetection { get; set; }

    internal Dictionary<string, CreateEmulatorSubscriptionOptions> SubscriptionOptions { get; } = [];

    public void AddSubscription(string name, Action<CreateEmulatorSubscriptionOptions>? configure = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        var options = new CreateEmulatorSubscriptionOptions();
        configure?.Invoke(options);

        SubscriptionOptions.TryAdd(name, options);
    }
}