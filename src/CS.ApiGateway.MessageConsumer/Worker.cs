using CS.ApiGateway.Core.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CS.ApiGateway.MessageConsumer
{
    public class Worker(ILogger<Worker> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory() { HostName = "cs.apigateway.rabbitmq" };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            // Declare (or check) the queue to consume from
            await channel.QueueDeclareAsync(
                queue: "order-queue",
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

                var order = new Order
                {
                    Id = 1,
                    Code = $"ORD-{DateTime.UtcNow.Date}",
                    User = userBasket.UserName,
                    TotalAmount = userBasket.BasketItems.Sum(x => x.Price * x.Quantity),
                    Products = []
                };

                order.Products = userBasket.BasketItems.Select((x, i) => new OrderLine
                {
                    OrderId = order.Id,
                    LineNumber = i,
                    Product = x.ProductName,
                    Amount = x.Price,
                    Quantity = x.Quantity }).ToList();



                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.ConnectionClose = true; //Set KeepAlive to false                
                    var url = "http://cs.apigateway.ordersmicroservice:6000/api/orders";
                    var serializedData = new StringContent(JsonSerializer.Serialize(order), Encoding.UTF8, "application/json");

                    var httpResponse = client.PostAsync(url, serializedData).Result; //Make sure it is synchonrous

                    var responseString = httpResponse.Content.ReadAsStringAsync().Result; //Make sure it is synchonrous
                }

                await ((AsyncEventingBasicConsumer)sender)
                    .Channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
            };
            await channel.BasicConsumeAsync("order-queue", autoAck: false, consumer);
        }
    }
}
