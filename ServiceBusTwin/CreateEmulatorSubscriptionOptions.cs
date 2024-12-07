namespace ServiceBusTwin;

public sealed class CreateEmulatorSubscriptionOptions
{
    public bool DeadLetteringOnMessageExpiration { get; set; }

    public TimeSpan DefaultMessageTimeToLive { get; set; } = TimeSpan.FromHours(1);

    public TimeSpan LockDuration { get; set; } = TimeSpan.FromMinutes(1);

    public int MaxDeliveryCount { get; set; } = 3;

    public string? ForwardDeadLetteredMessagesTo { get; set; }

    public string? ForwardTo { get; set; }

    public bool RequiresSession { get; set; }

    internal Dictionary<string, CreateEmulatorRuleOptions> RuleOptions { get; } = [];

    public void AddRule(string name, Action<CreateEmulatorRuleOptions>? configure = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        var options = new CreateEmulatorRuleOptions();
        configure?.Invoke(options);

        RuleOptions.TryAdd(name, options);
    }
}