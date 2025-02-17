using System.Threading.Tasks;

namespace Shop.Services.RewardApi.Messaging
{
    public interface IAzureServiceBusConsumer 
    {
        Task Start();
        Task Stop();
    }
}
