public class Program
{
    public static async Task Main(string[] args)
    {
        var producer = new ProducerFactory("persistent://public/default/mytopic", "Sub1");
        await producer.StartMessaging();
    }
}
