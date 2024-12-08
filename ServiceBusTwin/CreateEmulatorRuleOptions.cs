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

public interface IRuleWithSystemPropertyFilter
{
    IRuleWithSystemPropertyFilter AddSystemPropertyFilter(SystemProperties properties, string value);
}

public interface IRuleWithUserPropertyFilter
{
    IRuleWithUserPropertyFilter AddUserPropertyFilter(string key, string value);
}

public sealed class CreateEmulatorRuleOptions : IRuleWithSystemPropertyFilter, IRuleWithUserPropertyFilter
{
    public RuleFilterType FilterType { get; set; }

    internal Dictionary<SystemProperties, string> SystemProperties { get; } = [];
    internal Dictionary<string, string>           UserProperties   { get; } = [];

    public IRuleWithSystemPropertyFilter AddSystemPropertyFilter(SystemProperties properties, string value)
    {
        SystemProperties.TryAdd(properties, value);
        return this;
    }

    public IRuleWithUserPropertyFilter AddUserPropertyFilter(string key, string value)
    {
        UserProperties.TryAdd(key, value);
        return this;
    }
}