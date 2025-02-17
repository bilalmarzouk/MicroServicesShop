using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Shop.Services.EmailAPI.Message;
using Shop.Services.EmailAPI.Model.Dto;
using Shop.Services.EmailAPI.Services;
using System.Text;
using System.Text.Json.Serialization;

namespace Shop.Services.EmailAPI.Messaging
{
 

    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectingString;
        private readonly string emailCartQueue;
        private readonly string emailRegistrationCartQueue;

        private readonly string orderCreatedTopic;
        private readonly string orderCreatedEmailSubsucribtion;

        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;


        private ServiceBusProcessor _emailCartProcessor;

        private ServiceBusProcessor _emailRegistrationProcessor;
        private ServiceBusProcessor _emailOrderPlacedProcessor;



        public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration;
            serviceBusConnectingString = _configuration.GetValue<string>("ApiSettings:ServiceBusConnectingString");

            emailCartQueue = _configuration.GetValue<string>("ApiSettings:TopicAndQueueNames:EmailShoppingCartQueue");
            emailRegistrationCartQueue = _configuration.GetValue<string>("ApiSettings:TopicAndQueueNames:EmailRegistrationCartQueue");
            var client = new ServiceBusClient(serviceBusConnectingString);
            _emailCartProcessor = client.CreateProcessor(emailCartQueue);
            _emailRegistrationProcessor = client.CreateProcessor(emailRegistrationCartQueue);
            _emailService = emailService;
            orderCreatedTopic = _configuration.GetValue<string>("ApiSettings:TopicAndQueueNames:OrderCreatedTopic");
            orderCreatedEmailSubsucribtion = _configuration.GetValue<string>("ApiSettings:TopicAndQueueNames:OrderCreated_Email_Subsucribtion");
            _emailOrderPlacedProcessor = client.CreateProcessor(orderCreatedTopic, orderCreatedEmailSubsucribtion);
        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestRecived;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
            _emailRegistrationProcessor.ProcessMessageAsync += OnEmailRegistrationRecived;
            _emailRegistrationProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailCartProcessor.StartProcessingAsync();
            await _emailRegistrationProcessor.StartProcessingAsync();

            _emailOrderPlacedProcessor.ProcessMessageAsync += OnOrderPlacedRequestRecived;
            _emailOrderPlacedProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailOrderPlacedProcessor.StartProcessingAsync();
        }

        private async Task OnEmailRegistrationRecived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            var email = JsonConvert.DeserializeObject<string>(body);
            try
            {
                await _emailService.RegisterUserEmailLog(email);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private  Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private  async Task OnEmailCartRequestRecived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            CartDto objMessage = JsonConvert.DeserializeObject<CartDto>(body); 
            try
            {
                await _emailService.EmailCartAndLog(objMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        private async Task OnOrderPlacedRequestRecived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            RewardMessage objMessage = JsonConvert.DeserializeObject<RewardMessage>(body);
            try
            {
                await _emailService.LogOrderPlaced(objMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task Stop()
        {
            await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();

            await _emailRegistrationProcessor.StopProcessingAsync();
            await _emailRegistrationProcessor.DisposeAsync();

            await _emailOrderPlacedProcessor.DisposeAsync();
            await _emailOrderPlacedProcessor.StopProcessingAsync();

        }
    }
}
