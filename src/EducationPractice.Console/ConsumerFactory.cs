public class ConsumerFactory
{
    public string Topic { get; set; }

    public string SubscriptionName { get; set; }

    public string ServiceUrl { get; set; }

    public ConsumerFactory(string topic, string subName, string serviceUrl)
    {
        Topic = topic;
        SubscriptionName = subName;
        ServiceUrl = serviceUrl;
    }

    public async Task<IConsumer<byte[]>> CreateConsumer()
    {
        var serverId = Guid.NewGuid().ToString()[..8];

        var client = await new PulsarClientBuilder().ServiceUrl(ServiceUrl).BuildAsync();

        Console.WriteLine($"Server [{serverId}] were satrted");

        var consumer = await client
            .NewConsumer()
            .SubscriptionName(SubscriptionName)
            .Topic(Topic)
            .SubscriptionType(SubscriptionType.Shared)
            .NegativeAckRedeliveryDelay(TimeSpan.FromSeconds(30))
            .SubscribeAsync();

        return consumer;
    }
}
