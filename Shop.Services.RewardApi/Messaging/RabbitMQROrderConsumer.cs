
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shop.Services.RewardApi.Message;
using Shop.Services.RewardApi.Services;
using System.Text;
using System.Threading.Channels;

namespace Shop.Services.RewardApi.Messaging
{
    public class RabbitMQROrderConsumer : BackgroundService
    {
        private readonly RewardService _rewardService;
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private RabbitMQ.Client.IChannel  _channel;
        string _queueName;
        private const string OrderCreated_RewardUpdateQueue = "RewardsUpdateQueue";
        private readonly string _exchangeName = "";
        public RabbitMQROrderConsumer(IConfiguration configuration, RewardService rewardService)
        {
            _rewardService = rewardService;
            _configuration = configuration;
            _exchangeName = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Password = "guest",
                UserName = "guest",
            };
            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
             _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult(); // Async channel creation

            //fanout  
            // _channel.ExchangeDeclareAsync(_configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic"), ExchangeType.Fanout);
            // _queueName = _channel.QueueDeclareAsync().GetAwaiter().GetResult().QueueName;
            //_channel.QueueBindAsync(_queueName, _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic"), "");

            //direct
            _channel.ExchangeDeclareAsync(_exchangeName, ExchangeType.Direct);
            _channel.QueueDeclareAsync(OrderCreated_RewardUpdateQueue,false,false,false,null).GetAwaiter().GetResult();
            _channel.QueueBindAsync(OrderCreated_RewardUpdateQueue, _exchangeName, "RewardsUpdate");


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
                _channel.BasicAckAsync(ea.DeliveryTag,false).GetAwaiter().GetResult();
            };
            //fanout
            // _channel.BasicConsumeAsync(_queueName,false,consumer).GetAwaiter().GetResult();

            //direct
             _channel.BasicConsumeAsync(OrderCreated_RewardUpdateQueue, false,consumer).GetAwaiter().GetResult();
            return Task.CompletedTask;
        }

        private async Task HandleMessage(RewardMessage rewardMessage)
        {
            await _rewardService.UpdateReward(rewardMessage);
        }
    }
}
