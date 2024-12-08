namespace ServiceBusTwin;

public enum RuleFilterType
{
    Correlation,
    Sql
}

internal enum SystemProperties
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
    IRuleWithSystemPropertyFilter WithContentType(string value);

    IRuleWithSystemPropertyFilter WithCorrelationId(string value);

    IRuleWithSystemPropertyFilter WithLabel(string value);

    IRuleWithSystemPropertyFilter WithMessageId(string value);

    IRuleWithSystemPropertyFilter WithReplyTo(string value);

    IRuleWithSystemPropertyFilter WithReplyToSessionId(string value);

    IRuleWithSystemPropertyFilter WithSessionId(string value);

    IRuleWithSystemPropertyFilter WithTo(string value);
}

public interface IRuleWithUserPropertyFilter
{
    IRuleWithUserPropertyFilter WithUserPropertyFilter(string key, string value);
}

public sealed class CreateEmulatorRuleOptions : IRuleWithSystemPropertyFilter, IRuleWithUserPropertyFilter
{
    public RuleFilterType FilterType { get; set; }

    internal Dictionary<SystemProperties, string> SystemProperties { get; } = [];
    internal Dictionary<string, string>           UserProperties   { get; } = [];

    public IRuleWithSystemPropertyFilter WithContentType(string value)
    {
        SystemProperties.TryAdd(ServiceBusTwin.SystemProperties.ContentType, value);
        return this;
    }

    public IRuleWithSystemPropertyFilter WithCorrelationId(string value)
    {
        SystemProperties.TryAdd(ServiceBusTwin.SystemProperties.CorrelationId, value);
        return this;
    }

    public IRuleWithSystemPropertyFilter WithLabel(string value)
    {
        SystemProperties.TryAdd(ServiceBusTwin.SystemProperties.Label, value);
        return this;
    }

    public IRuleWithSystemPropertyFilter WithMessageId(string value)
    {
        SystemProperties.TryAdd(ServiceBusTwin.SystemProperties.MessageId, value);
        return this;
    }

    public IRuleWithSystemPropertyFilter WithReplyTo(string value)
    {
        SystemProperties.TryAdd(ServiceBusTwin.SystemProperties.ReplyTo, value);
        return this;
    }

    public IRuleWithSystemPropertyFilter WithReplyToSessionId(string value)
    {
        SystemProperties.TryAdd(ServiceBusTwin.SystemProperties.ReplyToSessionId, value);
        return this;
    }

    public IRuleWithSystemPropertyFilter WithSessionId(string value)
    {
        SystemProperties.TryAdd(ServiceBusTwin.SystemProperties.SessionId, value);
        return this;
    }

    public IRuleWithSystemPropertyFilter WithTo(string value)
    {
        SystemProperties.TryAdd(ServiceBusTwin.SystemProperties.To, value);
        return this;
    }

    public IRuleWithUserPropertyFilter WithUserPropertyFilter(string key, string value)
    {
        UserProperties.TryAdd(key, value);
        return this;
    }
}