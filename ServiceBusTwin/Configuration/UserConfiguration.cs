namespace ServiceBusTwin.Configuration;

internal sealed class UserConfiguration
{
    public required IList<NamespaceConfiguration> Namespaces { get; set; }

    public required LoggingConfiguration Logging { get; set; }
}