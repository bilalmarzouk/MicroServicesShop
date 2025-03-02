namespace Shop.Services.OrderApi.RabbitMQSender
{
    public interface IRabiitMQCartMessageSender
    {
        Task SendMessage(Object message, string exchangeName);
    }
}
