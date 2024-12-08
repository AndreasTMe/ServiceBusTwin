namespace ServiceBusTwin;

public sealed class ServiceBusEmulatorBuilder
{
    private readonly ServiceBusEmulatorConfiguration _configuration = new()
    {
        ConfigurationFile = new ConfigurationFile
        {
            UserConfig = new UserConfiguration
            {
                Namespaces =
                [
                    new NamespaceConfiguration
                    {
                        Name   = "sbemulatorns",
                        Queues = new HashSet<QueueConfiguration>(),
                        Topics = new HashSet<TopicConfiguration>()
                    }
                ],
                Logging = new LoggingConfiguration
                {
                    Type = "File"
                }
            }
        }
    };

    public ServiceBusEmulatorBuilder WithServiceBusEmulatorImage(string image)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(image);
        _configuration.SbEmulatorImage = image;
        return this;
    }

    public ServiceBusEmulatorBuilder WithServiceBusEmulatorName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        _configuration.SbEmulatorName = name;
        return this;
    }

    public ServiceBusEmulatorBuilder WithServiceBusEmulatorPort(int port)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(port, 1024, nameof(port));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(port, 65535, nameof(port));
        _configuration.SbEmulatorPort = port;
        return this;
    }

    public ServiceBusEmulatorBuilder WithQueue(Action<CreateEmulatorQueueOptions>? configure = default) =>
        WithQueue("default", configure);

    public ServiceBusEmulatorBuilder WithQueue(string name, Action<CreateEmulatorQueueOptions>? configure = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        var queueOptions = new CreateEmulatorQueueOptions();
        configure?.Invoke(queueOptions);

        _configuration.ConfigurationFile.UserConfig.Namespaces[0]
            .Queues
            .Add(ParseQueueConfiguration(name, queueOptions));

        return this;
    }

    public ServiceBusEmulatorBuilder WithTopic(Action<CreateEmulatorTopicOptions>? configure = default) =>
        WithTopic("default", configure);

    public ServiceBusEmulatorBuilder WithTopic(string name, Action<CreateEmulatorTopicOptions>? configure = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        var topicOptions = new CreateEmulatorTopicOptions();
        configure?.Invoke(topicOptions);

        _configuration.ConfigurationFile.UserConfig.Namespaces[0]
            .Topics
            .Add(ParseTopicConfiguration(name, topicOptions));

        return this;
    }

    public ServiceBusEmulatorBuilder WithLogging(EmulatorLogging logging)
    {
        _configuration.ConfigurationFile.UserConfig.Logging.Type = logging switch
        {
            EmulatorLogging.File           => "File",
            EmulatorLogging.Console        => "Console",
            EmulatorLogging.FileAndConsole => "File,Console",
            _                              => throw new ArgumentOutOfRangeException(nameof(logging), logging, null)
        };
        return this;
    }

    public IEmulator Build() => new ServiceBusEmulator(_configuration);

    private static QueueConfiguration ParseQueueConfiguration(string name, CreateEmulatorQueueOptions options) =>
        new()
        {
            Name = name,
            Properties = new QueueProperties
            {
                DeadLetteringOnMessageExpiration    = options.DeadLetteringOnMessageExpiration,
                DefaultMessageTimeToLive            = options.DefaultMessageTimeToLive.ToIso8601String(),
                DuplicateDetectionHistoryTimeWindow = options.DuplicateDetectionHistoryTimeWindow.ToIso8601String(),
                ForwardDeadLetteredMessagesTo       = options.ForwardDeadLetteredMessagesTo,
                ForwardTo                           = options.ForwardTo,
                LockDuration                        = options.LockDuration.ToIso8601String(),
                MaxDeliveryCount                    = options.MaxDeliveryCount,
                RequiresDuplicateDetection          = options.RequiresDuplicateDetection,
                RequiresSession                     = options.RequiresSession
            }
        };

    private static TopicConfiguration ParseTopicConfiguration(string name, CreateEmulatorTopicOptions options) =>
        new()
        {
            Name = name,
            Properties = new TopicProperties
            {
                DefaultMessageTimeToLive            = options.DefaultMessageTimeToLive.ToIso8601String(),
                DuplicateDetectionHistoryTimeWindow = options.DuplicateDetectionHistoryTimeWindow.ToIso8601String(),
                RequiresDuplicateDetection          = options.RequiresDuplicateDetection
            },
            Subscriptions = options.SubscriptionOptions.Select(ParseSubscriptionConfiguration).ToHashSet()
        };

    private static SubscriptionConfiguration ParseSubscriptionConfiguration(
        KeyValuePair<string, CreateEmulatorSubscriptionOptions> kvp)
    {
        var options = kvp.Value;
        return new SubscriptionConfiguration
        {
            Name = kvp.Key,
            Properties = new SubscriptionProperties
            {
                DeadLetteringOnMessageExpiration = options.DeadLetteringOnMessageExpiration,
                DefaultMessageTimeToLive         = options.DefaultMessageTimeToLive.ToIso8601String(),
                LockDuration                     = options.LockDuration.ToIso8601String(),
                MaxDeliveryCount                 = options.MaxDeliveryCount,
                ForwardDeadLetteredMessagesTo    = options.ForwardDeadLetteredMessagesTo,
                ForwardTo                        = options.ForwardTo,
                RequiresSession                  = options.RequiresSession
            },
            Rules = options.RuleOptions?.Select(ParseRuleConfiguration).ToHashSet() ?? null
        };
    }

    private static RuleConfiguration ParseRuleConfiguration(KeyValuePair<string, CreateEmulatorRuleOptions> kvp)
    {
        var options = kvp.Value;
        return new RuleConfiguration
        {
            Name = kvp.Key,
            Properties = new RuleProperties
            {
                FilterType = options.FilterType switch
                {
                    RuleFilterType.Correlation => "Correlation",
                    RuleFilterType.Sql         => throw new NotSupportedException("Sql filters not yet supported"),
                    _                          => throw new ArgumentOutOfRangeException(nameof(options.FilterType))
                },
                CorrelationFilter = new CorrelationFilter
                {
                    ContentType      = options.SystemProperties.GetValueOrDefault(SystemProperties.ContentType),
                    CorrelationId    = options.SystemProperties.GetValueOrDefault(SystemProperties.CorrelationId),
                    Label            = options.SystemProperties.GetValueOrDefault(SystemProperties.Label),
                    MessageId        = options.SystemProperties.GetValueOrDefault(SystemProperties.MessageId),
                    ReplyTo          = options.SystemProperties.GetValueOrDefault(SystemProperties.ReplyTo),
                    ReplyToSessionId = options.SystemProperties.GetValueOrDefault(SystemProperties.ReplyToSessionId),
                    SessionId        = options.SystemProperties.GetValueOrDefault(SystemProperties.SessionId),
                    To               = options.SystemProperties.GetValueOrDefault(SystemProperties.To),
                    Properties       = options.UserProperties
                }
            }
        };
    }
}