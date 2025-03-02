namespace Shop.Services.AuthApi.RabbitMQSender
{
    public interface IRabiitMQAuthMessageSender
    {
        Task SendMessage(Object message, string queueName);
    }
}
