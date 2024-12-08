namespace ServiceBusTwin.Configuration;

internal sealed class QueueProperties
{
    public bool DeadLetteringOnMessageExpiration { get; set; }

    public string DefaultMessageTimeToLive { get; set; } = "PT1H";

    public string DuplicateDetectionHistoryTimeWindow { get; set; } = "PT20S";

    public string ForwardDeadLetteredMessagesTo { get; set; } = string.Empty;

    public string ForwardTo { get; set; } = string.Empty;

    public string LockDuration { get; set; } = "PT1M";

    public int MaxDeliveryCount { get; set; } = 3;

    public bool RequiresDuplicateDetection { get; set; }

    public bool RequiresSession { get; set; }
}