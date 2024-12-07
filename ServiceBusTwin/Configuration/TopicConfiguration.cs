namespace ServiceBusTwin.Configuration;

internal sealed class TopicConfiguration : IEqualityComparer<TopicConfiguration>
{
    public required string Name { get; set; }

    public required TopicProperties Properties { get; set; }

    public required ISet<SubscriptionConfiguration> Subscriptions { get; set; }

    public bool Equals(TopicConfiguration? x, TopicConfiguration? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null) return false;
        if (y is null) return false;

        return x.Name == y.Name && x.GetHashCode() == y.GetHashCode();
    }

    public int GetHashCode(TopicConfiguration obj) => obj.Name.GetHashCode();
}