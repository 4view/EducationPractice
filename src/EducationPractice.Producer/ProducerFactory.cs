public class ProducerFactory
{
    public string Topic { get; set; }

    public string SubscriptionName { get; set; }

    public ProducerFactory(string topic, string subName)
    {
        Topic = topic;
        SubscriptionName = subName;
    }

    public async Task StartMessaging()
    {
        try
        {
            Console.WriteLine($"Connecting to the Pulsar server");

            var client = await new PulsarClientBuilder()
                .ServiceUrl("pulsar://localhost:6650")
                .BuildAsync();

            Console.WriteLine("Connection to the server has been successfully established");

            var pulsarConfig = new PulsarConfig()
            {
                Topic = this.Topic,
                SubscriptionName = this.SubscriptionName,
            };

            try
            {
                Console.WriteLine("Producer creation...");

                var rand = new Random();
                var producer = await client.NewProducer().Topic(pulsarConfig.Topic).CreateAsync();

                Console.WriteLine("Producer has been created \nStart sending message....\n");

                for (int i = 1; i <= 10; i++)
                {
                    var number = rand.Next(-100, 100).ToString();

                    await producer.SendAsync(Encoding.UTF8.GetBytes(number));
                    Console.WriteLine($"Message {i} was sent with content: {number}");
                    await Task.Delay(2000);
                }
                Console.WriteLine("All messages was sent!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while creating producer: {ex}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Pulsar connection error: {ex}");
        }
    }
}
