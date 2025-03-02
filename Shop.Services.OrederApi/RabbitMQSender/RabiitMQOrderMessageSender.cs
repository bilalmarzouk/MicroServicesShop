using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Threading;
using System.Threading.Channels;

namespace Shop.Services.OrderApi.RabbitMQSender
{
    public class RabiitMQOrderMessageSender : IRabiitMQCartMessageSender
    {
        private readonly string _hostName;
        private readonly string _userName;
        private readonly string _password;
        private IConnection _connection;

        private const string  OrderCreated_RewardUpdateQueue = "RewardsUpdateQueue";
        private const string OrderCreated_EmailUpdateQueue = "EmailUpdateQueue";
        public RabiitMQOrderMessageSender()
        {
            _hostName = "localhost";
            _password = "guest";
            _userName = "guest";
        }
        public async Task SendMessage(object message, string exchangeName)
        {

            if (await ConnectionExits())

            {
                using var channel = await _connection.CreateChannelAsync(); // Async channel creation
                //fanout   
                // await channel.ExchangeDeclareAsync(exchangeName,ExchangeType.Fanout,durable:false);

                //direct
                   await channel.ExchangeDeclareAsync(exchangeName,ExchangeType.Direct,durable:false);
                  await channel.QueueDeleteAsync(OrderCreated_RewardUpdateQueue,false,false,false,cancellationToken:CancellationToken.None);
                await channel.QueueDeleteAsync(OrderCreated_EmailUpdateQueue, false, false, false, cancellationToken: CancellationToken.None);

                channel.QueueBindAsync(OrderCreated_EmailUpdateQueue, exchangeName, "EmailUpdate");
                channel.QueueBindAsync(OrderCreated_EmailUpdateQueue, exchangeName, "RewardsUpdate");

                var json = JsonConvert.SerializeObject(message);
                    var body = Encoding.UTF8.GetBytes(json);
                    var properties = new BasicProperties();

                //fanout
                //    await channel.BasicPublishAsync(
                //exchange: exchangeName,
                //routingKey: "",
                //mandatory: false,
                // properties, // Cannot be `null` in the new API
                //    body: body
                //);

                //direct
                await channel.BasicPublishAsync(
            exchange: exchangeName,
            routingKey: "EmailUpdate",
            mandatory: false,
             properties, // Cannot be `null` in the new API
                body: body
            );
                await channel.BasicPublishAsync(
       exchange: exchangeName,
       routingKey: "RewardsUpdate",
       mandatory: false,
        properties, // Cannot be `null` in the new API
           body: body
       );
                Console.WriteLine("RabbitMQ connection and channel established asynchronously.");
                
            }
        }

        private async Task CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostName,
                    UserName = _userName,
                    Password = _password,


                };
                _connection = await factory.CreateConnectionAsync();
            }
            catch (Exception ex)
            {
            }
        }

        private async Task<bool> ConnectionExits()
        {
            if (_connection != null)
            {
                return true;
            }
            await CreateConnection();
            return true;
        }

    }
}
