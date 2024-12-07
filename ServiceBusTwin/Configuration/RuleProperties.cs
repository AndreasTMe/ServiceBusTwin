namespace ServiceBusTwin.Configuration;

internal sealed class RuleProperties
{
    public string? FilterType { get; set; }

    public CorrelationFilter? CorrelationFilter { get; set; }
}