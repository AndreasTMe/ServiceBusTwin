namespace ServiceBusTwin;

public sealed class EmulatorBuilder
{
    private readonly EmulatorConfiguration _configuration = new()
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

    public EmulatorBuilder WithNetwork(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        _configuration.NetworkName = name;
        return this;
    }

    public EmulatorBuilder WithServiceBusEmulatorImage(string image)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(image);
        _configuration.SbEmulatorImage = image;
        return this;
    }

    public EmulatorBuilder WithServiceBusEmulatorName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        _configuration.SbEmulatorName = name;
        return this;
    }

    public EmulatorBuilder WithSqlServerImage(string image)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(image);
        _configuration.SqlServerImage = image;
        return this;
    }

    public EmulatorBuilder WithSqlServerName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        _configuration.SqlServerName = name;
        return this;
    }

    public EmulatorBuilder WithSqlServerSaPassword(string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);
        _configuration.SqlSaPassword = password;
        return this;
    }

    public EmulatorBuilder WithQueue(Action<CreateEmulatorQueueOptions>? configure = default) =>
        WithQueue("default", configure);

    public EmulatorBuilder WithQueue(string name, Action<CreateEmulatorQueueOptions>? configure = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        var queueOptions = new CreateEmulatorQueueOptions();
        configure?.Invoke(queueOptions);

        _configuration.ConfigurationFile.UserConfig.Namespaces[0]
                      .Queues
                      .Add(ParseQueueConfiguration(name, queueOptions));

        return this;
    }

    public EmulatorBuilder WithTopic(Action<CreateEmulatorTopicOptions>? configure = default) =>
        WithTopic("default", configure);

    public EmulatorBuilder WithTopic(string name, Action<CreateEmulatorTopicOptions>? configure = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        var topicOptions = new CreateEmulatorTopicOptions();
        configure?.Invoke(topicOptions);

        _configuration.ConfigurationFile.UserConfig.Namespaces[0]
                      .Topics
                      .Add(ParseTopicConfiguration(name, topicOptions));

        return this;
    }

    public EmulatorBuilder WithLogging(EmulatorLogging logging)
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
            Rules = options.RuleOptions.Select(ParseRuleConfiguration).ToHashSet()
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
#pragma warning disable CS8524 // The switch expression does not handle some values of its input type (it is not exhaustive) involving an unnamed enum value.
                FilterType = options.FilterType switch
#pragma warning restore CS8524 // The switch expression does not handle some values of its input type (it is not exhaustive) involving an unnamed enum value.
                {
                    RuleFilterType.Correlation => "Correlation",
                    RuleFilterType.Sql         => "Sql"
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