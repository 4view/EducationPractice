using Pulsar.Client.Common;

public class ConsumerFactory
{
    public string Topic { get; set; }

    public string SubscriptionName { get; set; }

    public ConsumerFactory(string topic, string subName)
    {
        Topic = topic;
        SubscriptionName = subName;
    }

    public async Task<IConsumer<byte[]>> CreateConsumer()
    {
        var serverId = Guid.NewGuid().ToString()[..8];

        var client = await new PulsarClientBuilder()
            .ServiceUrl("pulsar://localhost:6650")
            .BuildAsync();

        var pulsarConfig = new PulsarConfig()
        {
            Topic = this.Topic,
            SubscriptionName = this.SubscriptionName,
        };

        Console.WriteLine($"Server [{serverId}] were satrted");

        var consumer = await client
            .NewConsumer()
            .SubscriptionName(pulsarConfig.SubscriptionName)
            .Topic(pulsarConfig.Topic)
            .SubscriptionType(SubscriptionType.Shared)
            .SubscribeAsync();

        return consumer;
    }
}
