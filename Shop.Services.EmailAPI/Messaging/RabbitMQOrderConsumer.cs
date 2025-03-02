
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shop.Services.EmailAPI.Message;
using Shop.Services.EmailAPI.Services;
using System.Text;
using System.Threading.Channels;

namespace Shop.Services.EmailAPI.Messaging
{
    public class RabbitMQOrderConsumer : BackgroundService
    {
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private RabbitMQ.Client.IChannel  _channel;
        string _queueName;
        private readonly string _exchangeName = "";
        private const string OrderCreated_EmailUpdateQueue = "EmailUpdateQueue";
        public RabbitMQOrderConsumer(IConfiguration configuration, EmailService emailService)
        {
            _emailService = emailService;
            _configuration = configuration;
            _exchangeName = _configuration.GetValue<string>("ApiSettings:TopicAndQueueNames:OrderCreatedTopic");
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Password = "guest",
                UserName = "guest",
            };
            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
             _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult(); // Async channel creation

            //fanout
            //    _channel.ExchangeDeclareAsync(_configuration.GetValue<string>("ApiSettings:TopicAndQueueNames:OrderCreatedTopic"), ExchangeType.Fanout);
            //_queueName = _channel.QueueDeclareAsync().GetAwaiter().GetResult().QueueName;
            //_channel.QueueBindAsync(_queueName, _configuration.GetValue<string>("ApiSettings:TopicAndQueueNames:OrderCreatedTopic"), "");

            //direct
            _channel.ExchangeDeclareAsync(_exchangeName, ExchangeType.Direct);
            _channel.QueueDeclareAsync(OrderCreated_EmailUpdateQueue, false, false, false, null).GetAwaiter().GetResult();
            _channel.QueueBindAsync(OrderCreated_EmailUpdateQueue, _exchangeName, "EmailUpdate");
        }
        protected override  Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                RewardMessage rewardMessage = JsonConvert.DeserializeObject<RewardMessage>(content);
                HandleMessage(rewardMessage);

              
            };
            //fanout
            //_channel.BasicAckAsync(ea.DeliveryTag,false).GetAwaiter().GetResult();

            //direct
            _channel.BasicConsumeAsync(OrderCreated_EmailUpdateQueue, false, consumer).GetAwaiter().GetResult();
            return Task.CompletedTask;
        }

        private async Task HandleMessage(RewardMessage rewardMessage)
        {
            await _emailService.LogOrderPlaced(rewardMessage);
        }
    }
}
