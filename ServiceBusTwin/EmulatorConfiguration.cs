namespace ServiceBusTwin;

public sealed class EmulatorConfiguration
{
    public string NetworkName { get; internal set; } = "sb-emulator-network";

    public string SbEmulatorImage { get; internal set; } =
        "mcr.microsoft.com/azure-messaging/servicebus-emulator:latest";

    public string SbEmulatorName { get; internal set; } = "sb-emulator";

    public string SqlServerImage { get; internal set; } = "mcr.microsoft.com/mssql/server";

    public string SqlServerName { get; internal set; } = "sb-database";

    public string SqlSaPassword { get; internal set; } = "Password123!";

    internal ConfigurationFile ConfigurationFile { get; init; } = default!;
}