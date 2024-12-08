namespace ServiceBusTwin;

public sealed class CreateEmulatorQueueOptions
{
    public bool DeadLetteringOnMessageExpiration { get; set; }

    public TimeSpan DefaultMessageTimeToLive { get; set; } = TimeSpan.FromHours(1);

    public TimeSpan DuplicateDetectionHistoryTimeWindow { get; set; } = TimeSpan.FromSeconds(20);

    public string ForwardDeadLetteredMessagesTo { get; set; } = string.Empty;

    public string ForwardTo { get; set; } = string.Empty;

    public TimeSpan LockDuration { get; set; } = TimeSpan.FromMinutes(1);

    public int MaxDeliveryCount { get; set; } = 3;

    public bool RequiresDuplicateDetection { get; set; }

    public bool RequiresSession { get; set; }
}