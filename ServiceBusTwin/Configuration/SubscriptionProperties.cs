namespace ServiceBusTwin.Configuration;

internal sealed class SubscriptionProperties
{
    public bool DeadLetteringOnMessageExpiration { get; set; }

    public string? DefaultMessageTimeToLive { get; set; } = "PT1H";

    public string? LockDuration { get; set; } = "PT1M";

    public int MaxDeliveryCount { get; set; } = 3;

    public string? ForwardDeadLetteredMessagesTo { get; set; }

    public string? ForwardTo { get; set; }

    public bool RequiresSession { get; set; }
}