namespace ServiceBusTwin;

public interface ISubscriptionWithRuleOptions
{
    ISubscriptionWithRuleOptions AddRule(string name, Action<CreateEmulatorRuleOptions>? configure = default);
}

public sealed class CreateEmulatorSubscriptionOptions : ISubscriptionWithRuleOptions
{
    public bool DeadLetteringOnMessageExpiration { get; set; }

    public TimeSpan DefaultMessageTimeToLive { get; set; } = TimeSpan.FromHours(1);

    public TimeSpan LockDuration { get; set; } = TimeSpan.FromMinutes(1);

    public int MaxDeliveryCount { get; set; } = 3;

    public string ForwardDeadLetteredMessagesTo { get; set; } = string.Empty;

    public string ForwardTo { get; set; } = string.Empty;

    public bool RequiresSession { get; set; }

    internal Dictionary<string, CreateEmulatorRuleOptions>? RuleOptions { get; private set; }

    public ISubscriptionWithRuleOptions AddRule(string name, Action<CreateEmulatorRuleOptions>? configure = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        var options = new CreateEmulatorRuleOptions();
        configure?.Invoke(options);

        RuleOptions ??= new Dictionary<string, CreateEmulatorRuleOptions>();
        RuleOptions.TryAdd(name, options);

        return this;
    }
}