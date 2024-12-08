namespace ServiceBusTwin.Configuration;

internal sealed class SubscriptionConfiguration : IEqualityComparer<SubscriptionConfiguration>
{
    public required string Name { get; set; }

    public required SubscriptionProperties Properties { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ISet<RuleConfiguration>? Rules { get; set; }

    public bool Equals(SubscriptionConfiguration? x, SubscriptionConfiguration? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null) return false;
        if (y is null) return false;

        return x.Name == y.Name && x.GetHashCode() == y.GetHashCode();
    }

    public int GetHashCode(SubscriptionConfiguration obj) => obj.Name.GetHashCode();
}