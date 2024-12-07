namespace ServiceBusTwin.Configuration;

internal sealed class QueueConfiguration : IEqualityComparer<QueueConfiguration>
{
    public required string Name { get; set; }

    public required QueueProperties Properties { get; set; }

    public bool Equals(QueueConfiguration? x, QueueConfiguration? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null) return false;
        if (y is null) return false;

        return x.Name == y.Name && x.GetHashCode() == y.GetHashCode();
    }

    public int GetHashCode(QueueConfiguration obj) => obj.Name.GetHashCode();
}