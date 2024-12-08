namespace ServiceBusTwin.Emulators;

internal sealed class ServiceBusEmulator : IEmulator
{
    private readonly INetwork   _network;
    private readonly IContainer _sqlServer;
    private readonly IContainer _serviceBus;

    public ServiceBusEmulator(EmulatorConfiguration configuration)
    {
        configuration.ConfigurationFile.SaveChanges(out var absoluteConfigPath);

        _network = new NetworkBuilder()
            .WithName(configuration.NetworkName)
            .Build();

        _sqlServer = new ContainerBuilder()
            .WithImage(configuration.SqlServerImage)
            .WithImagePullPolicy(PullPolicy.Always)
            .WithName(configuration.SqlServerName)
            .WithNetwork(_network)
            .WithNetworkAliases(configuration.SqlServerName)
            .WithEnvironment("ACCEPT_EULA", "Y")
            .WithEnvironment("MSSQL_SA_PASSWORD", configuration.SqlSaPassword)
            .Build();

        _serviceBus = new ContainerBuilder()
            .WithImage(configuration.SbEmulatorImage)
            .WithImagePullPolicy(PullPolicy.Always)
            .WithName(configuration.SbEmulatorName)
            .WithPortBinding(5672, 5672)
            .WithNetwork(_network)
            .WithNetworkAliases(configuration.SbEmulatorName)
            .WithEnvironment("ACCEPT_EULA", "Y")
            .WithEnvironment("SQL_SERVER", configuration.SqlServerName)
            .WithEnvironment("MSSQL_SA_PASSWORD", configuration.SqlSaPassword)
            .WithResourceMapping(absoluteConfigPath, "/")
            .WithEnvironment("CONFIG_PATH", "/config.json")
            .WithWaitStrategy(
                Wait.ForUnixContainer()
                    .UntilMessageIsLogged("Emulator Service is Successfully Up!"))
            .DependsOn(_sqlServer)
            .Build();
    }

    public async Task StartAsync()
    {
        await _network.CreateAsync();
        await _sqlServer.StartAsync();
        await _serviceBus.StartAsync();
    }

    public async Task StopAsync()
    {
        await _serviceBus.StopAsync();
        await _sqlServer.StopAsync();
        await _network.DeleteAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _serviceBus.DisposeAsync();
        await _sqlServer.DisposeAsync();
        await _network.DisposeAsync();
    }
}