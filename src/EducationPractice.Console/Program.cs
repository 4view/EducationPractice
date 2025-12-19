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

        var consumerFactory = new ConsumerFactory("persistent://public/default/mytopic", "Sub1");

        var consumer = await consumerFactory.CreateConsumer();

        await counter.Start(consumer);
    }
}
