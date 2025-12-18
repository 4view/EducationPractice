public class Program
{
    public static async Task Main(string[] args)
    {
        var counter = new Counter();
        counter.OnNumIncreased += (message) => Console.WriteLine(message);
        counter.OnGoalAchieved += (message) => Console.WriteLine(message);
        counter.OnCounterReset += (message) => Console.WriteLine(message);
        counter.OnNumDecreased += (message) => Console.WriteLine(message);
        counter.OnGoalLost += (message) => Console.WriteLine(message);

        var consumerFactory = new ConsumerFactory("Sub1", "persistent://public/default/mytopic");

        var consumer = await consumerFactory.CreateConsumer();

        await counter.Start(consumer);
    }
}

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
            .SubscribeAsync();

        return consumer;
    }
}
