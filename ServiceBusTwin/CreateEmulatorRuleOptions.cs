namespace ServiceBusTwin;

public enum RuleFilterType
{
    Correlation,
    Sql
}

public enum SystemProperties
{
    ContentType,
    CorrelationId,
    Label,
    MessageId,
    ReplyTo,
    ReplyToSessionId,
    SessionId,
    To
}

public sealed class CreateEmulatorRuleOptions
{
    public RuleFilterType FilterType { get; set; }

    internal Dictionary<SystemProperties, string> SystemProperties { get; } = [];
    internal Dictionary<string, string>           UserProperties   { get; } = [];

    public void AddSystemPropertyFilter(SystemProperties properties, string value) =>
        SystemProperties.TryAdd(properties, value);

    public void AddUserPropertyFilter(string key, string value) => UserProperties.TryAdd(key, value);
}