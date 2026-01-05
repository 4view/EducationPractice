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

        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        var pulsarSettings = config.GetSection("PulsarConfig");

        var topic = pulsarSettings["Topic"] ?? "persistent://public/default/myTopic";
        var subName = pulsarSettings["SubscriptionName"] ?? "Sub1";
        var serviceUrl = pulsarSettings["ServiceUrl"] ?? "pulsar://localhost:6650";

        if (args.Length == 0)
        {
            Console.WriteLine("Usage: dotnet run [p || c] \np - for producer\nc - for consumer");
        }

        var mode = args[0];

        if (mode is "p")
        {
            var producer = new ProducerFactory(topic, serviceUrl);
            await producer.StartMessaging();
        }
        else if (mode is "c")
        {
            var consumerFactory = new ConsumerFactory(topic, subName, serviceUrl);
            var consumer = await consumerFactory.CreateConsumer();
            await counter.Start(consumer);
        }
        else
        {
            Console.WriteLine($"Unknown mode: {mode}. Use [c] or [p]");
        }
    }
}
