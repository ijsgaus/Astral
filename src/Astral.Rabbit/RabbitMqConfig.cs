using System;
using System.Threading;
using RabbitLink.Configuration;

namespace Astral.Rabbit
{
    public class RabbitMqConfig
    {
        public string Url { get; set; }

        public TimeSpan ConnectionTimeout { get; set; } = TimeSpan.FromSeconds(10);

        public TimeSpan? RecoveryInterval { get; set; } = TimeSpan.FromSeconds(3);

        public TimeSpan PublishTimeout { get; set; } = TimeSpan.FromMinutes(2);

        public bool AutoStart { get; set; } = true;
        public TimeSpan? ChannelRecoveryInterval { get; set; } = TimeSpan.FromSeconds(10);
        public TimeSpan ConnectionRecoveryInterval { get; set; } = TimeSpan.FromSeconds(10);

        public bool ConsumerAutoAck { get; set; } = false;
        public bool ConsumerCancelOnHaFailover { get; set; } = false;

        public TimeSpan ConsumerGetMessageTimeout { get; set; } = Timeout.InfiniteTimeSpan;

        public ushort ConsumerPrefetchCount { get; set; } = 1;

        public bool ProducerConfirmsMode { get; set; } = true;

        public ILinkConfigurationBuilder Apply(ILinkConfigurationBuilder cfg)
        {
            cfg.ConnectionTimeout(ConnectionTimeout);
            cfg.ProducerPublishTimeout(PublishTimeout);
            cfg.AutoStart(AutoStart);
            cfg.ChannelRecoveryInterval(ConnectionRecoveryInterval);
            cfg.ConsumerAutoAck(ConsumerAutoAck);
            cfg.ConsumerCancelOnHaFailover(ConsumerCancelOnHaFailover);

            if (ConsumerGetMessageTimeout > TimeSpan.Zero)
                cfg.ConsumerGetMessageTimeout(ConsumerGetMessageTimeout);
            cfg.ConsumerPrefetchCount(ConsumerPrefetchCount);
            cfg.ProducerConfirmsMode(ProducerConfirmsMode);
            if (ChannelRecoveryInterval != null)
                cfg.ChannelRecoveryInterval(ChannelRecoveryInterval.Value);
            if (RecoveryInterval != null)
                cfg.TopologyRecoveryInterval(RecoveryInterval.Value);
            return cfg;
        }
    }
}