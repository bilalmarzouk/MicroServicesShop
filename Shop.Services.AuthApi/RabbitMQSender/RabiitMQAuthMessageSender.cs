using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Threading;
using System.Threading.Channels;

namespace Shop.Services.AuthApi.RabbitMQSender
{
    public class RabiitMQAuthMessageSender : IRabiitMQAuthMessageSender
    {
        private readonly string _hostName;
        private readonly string _userName;
        private readonly string _password;
        private IConnection _connection;
        public RabiitMQAuthMessageSender()
        {
            _hostName = "localhost";
            _password = "guest";
            _userName = "guest";
        }
        public async Task SendMessage(object message, string queueName)
        {

            if (await ConnectionExits())

            {
                using var channel = await _connection.CreateChannelAsync(); // Async channel creation
            
                    await channel.QueueDeclareAsync(queueName, false, false, false, null);
                    var json = JsonConvert.SerializeObject(message);
                    var body = Encoding.UTF8.GetBytes(json);
                    var properties = new BasicProperties();
                    await channel.BasicPublishAsync(
                exchange: "",
                routingKey: queueName,
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
