namespace Shop.Services.ShoppingCart.RabbitMQSender
{
    public interface IRabiitMQCartMessageSender
    {
        Task SendMessage(Object message, string queueName);
    }
}
