public class ProducerFactory
{
    public string Topic { get; set; }

    public string ServiceUrl { get; set; }

    public ProducerFactory(string topic, string serviceUrl)
    {
        Topic = topic;
        ServiceUrl = serviceUrl;
    }

    /// <summary>
    /// Создает объект продюсера и начинает отправлять сообщения
    /// </summary>
    public async Task StartMessaging()
    {
        var rand = new Random();
        var producer = await CreateProducer();

        Console.WriteLine("Producer has been created \nStart sending message....\n");

        try
        {
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
            Console.WriteLine($"Error while sending messages: {ex}");
        }
    }

    private async Task<IProducer<byte[]>> CreateProducer()
    {
        try
        {
            Console.WriteLine("Connecting to the pulsar server");

            var client = await new PulsarClientBuilder().ServiceUrl(ServiceUrl).BuildAsync();

            Console.WriteLine("Connection to the server has been successfully established");

            try
            {
                Console.WriteLine("Creating producer....");

                var producer = await client.NewProducer().Topic(Topic).CreateAsync();

                return producer;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while creating producer: {ex}");
                throw;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Pulsar connection error: {ex}");
            throw;
        }
    }
}
