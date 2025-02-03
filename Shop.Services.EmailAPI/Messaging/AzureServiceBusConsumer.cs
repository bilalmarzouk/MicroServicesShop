using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
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
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;


        private ServiceBusProcessor _emailCartProcessor;

        private ServiceBusProcessor _emailRegistrationProcessor;



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

        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestRecived;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
            _emailRegistrationProcessor.ProcessMessageAsync += OnEmailRegistrationRecived;
            _emailRegistrationProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailCartProcessor.StartProcessingAsync();
            await _emailRegistrationProcessor.StartProcessingAsync();
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

        public async Task Stop()
        {
            await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();
            _emailRegistrationProcessor.StopProcessingAsync();
            _emailRegistrationProcessor.DisposeAsync();
        }
    }
}
