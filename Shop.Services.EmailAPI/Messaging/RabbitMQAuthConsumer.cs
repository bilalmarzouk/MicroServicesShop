
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shop.Services.EmailAPI.Services;
using System.Text;
using System.Threading.Channels;

namespace Shop.Services.EmailAPI.Messaging
{
    public class RabbitMQAuthConsumer : BackgroundService
    {
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private RabbitMQ.Client.IChannel  _channel;
        public RabbitMQAuthConsumer(IConfiguration configuration, EmailService emailService)
        {
            _emailService = emailService;
            _configuration = configuration;
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Password = "guest",
                UserName = "guest",
            };
            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
             _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult(); // Async channel creation
            
                _channel.QueueDeclareAsync(_configuration.GetValue<string>("ApiSettings:TopicAndQueueNames:EmailRegistrationCartQueue"), false, false, false, null);
            
        }
        protected override  Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                string email = JsonConvert.DeserializeObject<string>(content);
                HandleMessage(email);
                _channel.BasicAckAsync(ea.DeliveryTag,false).GetAwaiter().GetResult();
            };
            _channel.BasicConsumeAsync(_configuration.GetValue<string>("ApiSettings:TopicAndQueueNames:EmailRegistrationCartQueue"),false,consumer).GetAwaiter().GetResult();
        return Task.CompletedTask;
        }

        private async Task HandleMessage(string email)
        {
            await _emailService.RegisterUserEmailLog(email);
        }
    }
}
