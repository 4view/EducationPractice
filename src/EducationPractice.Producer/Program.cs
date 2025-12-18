public class Program
{
    public static async Task Main(string[] args)
    {
        var producer = new ProducerFactory("Sub1", "persistent://public/default/mytopic");
        producer.StartMessaging();
    }
}
