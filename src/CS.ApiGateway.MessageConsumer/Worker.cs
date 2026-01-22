using CS.ApiGateway.Core.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace CS.ApiGateway.MessageConsumer
{
    public class Worker : BackgroundService
    {
        private readonly string? rabbitMqQueue;
        private readonly string? rabbitMqHost;
        private readonly string? ordersMicroserviceUrl;
        private IConnection? connection;
        private IChannel? channel;
        private readonly ILogger logger;
        private readonly ConnectionFactory? connectionFactory;
        private static int ordercounter = 1;
        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.rabbitMqHost = configuration["ServicesReferences:RabbitMqHost"];
            this.rabbitMqQueue = configuration["ServicesReferences:RabbitMqQueue"];
            this.ordersMicroserviceUrl = configuration["ServicesReferences:OrdersMicroserviceUrl"];

            if (!string.IsNullOrEmpty(rabbitMqHost))
            {
                this.connectionFactory = new ConnectionFactory() { HostName = rabbitMqHost };
            }

            Task.Run(InitializeQueue);
        }
        private async void InitializeQueue()
        {

            this.connection = this.connectionFactory != null ? await this.connectionFactory.CreateConnectionAsync() : null;
            this.channel = connection != null ? await connection.CreateChannelAsync() : null;
            if (connection == null || channel == null || string.IsNullOrWhiteSpace(rabbitMqQueue)) return;
            // Declare (or check) the queue to consume from
            await channel.QueueDeclareAsync(
                queue: rabbitMqQueue,
                durable: true, // must match the producer's queue settings
                exclusive: false, // can be used by other connections
                autoDelete: false, // don’t delete when the last consumer disconnects
                arguments: null);

            // Define a consumer and start listening
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (sender, eventArgs) =>
            {
                byte[] body = eventArgs.Body.ToArray();
                string userBasketString = Encoding.UTF8.GetString(body);

                var userBasket = JsonSerializer.Deserialize<User>(userBasketString);


                if (userBasket != null)
                {
                    ++ordercounter;
                    var order = new Order
                    {
                        Code = $"ORD-{DateTime.UtcNow.Date:ddMMyyyy}-{ordercounter}",
                        User = userBasket.UserName,
                        TotalAmount = userBasket.BasketItems.Sum(x => x.Price * x.Quantity),
                        OrderLines = userBasket.BasketItems.Select((x, i) => new OrderLine
                        {
                            LineNumber = i,
                            Product = x.ProductName,
                            Amount = x.Price,
                            Quantity = x.Quantity
                        }).ToList()
                    };

                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.ConnectionClose = true; //Set KeepAlive to false                
                        var serializedData = new StringContent(JsonSerializer.Serialize(order), Encoding.UTF8, "application/json");

                        var httpResponse = client.PostAsync(ordersMicroserviceUrl, serializedData).Result; 

                        var responseString = httpResponse.Content.ReadAsStringAsync().Result;
                    }
                }

                await ((AsyncEventingBasicConsumer)sender)
                    .Channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
            };

            await channel.BasicConsumeAsync(rabbitMqQueue, autoAck: false, consumer);


        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {

                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
