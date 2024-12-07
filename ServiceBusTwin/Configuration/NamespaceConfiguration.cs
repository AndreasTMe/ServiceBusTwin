namespace ServiceBusTwin.Configuration;

internal sealed class NamespaceConfiguration
{
    public required string Name { get; set; }

    public required ISet<QueueConfiguration> Queues { get; set; }

    public required ISet<TopicConfiguration> Topics { get; set; }
}