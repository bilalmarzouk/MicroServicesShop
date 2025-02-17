using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Shop.Services.RewardApi.Message;
using Shop.Services.RewardApi.Services;
using System.Text;
using System.Text.Json.Serialization;

namespace Shop.Services.RewardApi.Messaging
{
 

    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectingString;
        private readonly string orderCreatedTopic;
        private readonly string orderCreatedRewardSubsucribtion;
        private readonly RewardService _rewardService;
        private readonly IConfiguration _configuration;


        private ServiceBusProcessor _rewardProcessor;





        public AzureServiceBusConsumer(IConfiguration configuration, RewardService rewardService)
        {
            _configuration = configuration;
            serviceBusConnectingString = _configuration.GetValue<string>("ServiceBusConnectingString");

            orderCreatedTopic = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
            orderCreatedRewardSubsucribtion = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreated_Rewards_Subsucribtion");
            var client = new ServiceBusClient(serviceBusConnectingString);
            _rewardProcessor = client.CreateProcessor(orderCreatedTopic, orderCreatedRewardSubsucribtion);
        
            _rewardService = rewardService;

        }

        public async Task Start()
        {
            _rewardProcessor.ProcessMessageAsync += OnNewOrderRewardRequestRecived;
            _rewardProcessor.ProcessErrorAsync += ErrorHandler;
        
            await _rewardProcessor.StartProcessingAsync();
          
        }

   

        private  Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private  async Task OnNewOrderRewardRequestRecived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            RewardMessage objMessage = JsonConvert.DeserializeObject<RewardMessage>(body); 
            try
            {
                await _rewardService.UpdateReward(objMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task Stop()
        {
            await _rewardProcessor.StopProcessingAsync();
            await _rewardProcessor.DisposeAsync();
           
        }
    }
}
