namespace ServiceBusTwin.Configuration;

internal sealed class CorrelationFilter
{
    public string? ContentType { get; set; }

    public string? CorrelationId { get; set; }

    public string? Label { get; set; }

    public string? MessageId { get; set; }

    public string? ReplyTo { get; set; }

    public string? ReplyToSessionId { get; set; }

    public string? SessionId { get; set; }

    public string? To { get; set; }

    public IDictionary<string, string>? Properties { get; set; }
}