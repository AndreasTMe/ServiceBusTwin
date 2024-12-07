namespace ServiceBusTwin.Configuration;

internal sealed class RuleConfiguration : IEqualityComparer<RuleConfiguration>
{
    public required string Name { get; set; }

    public required RuleProperties Properties { get; set; }

    public bool Equals(RuleConfiguration? x, RuleConfiguration? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null) return false;
        if (y is null) return false;

        return x.Name == y.Name && x.GetHashCode() == y.GetHashCode();
    }

    public int GetHashCode(RuleConfiguration obj) => obj.Name.GetHashCode();
}